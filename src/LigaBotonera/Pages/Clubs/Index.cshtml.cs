using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using LigaBotonera.Entities;
using LigaBotonera.Pages.Shared;
using LigaBotonera.Pages.Shared.Lookup;
using LigaBotonera.Pages.Shared.ModalDialog;
using LigaBotonera.Persistence;
using LigaBotonera.ViewComponents.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LigaBotonera.Pages.Clubs;
public class Index(ApplicationDbContext dbContext) : PageModel
{
    [BindProperty]
    public ViewModel Data { get; set; } = new();

    public IList<Club> Clubs { get; set; } = [];

    public PaginationViewModel Pagination { get; set; } = new();

    public LookupViewModel CityLookup = new()
    {
        Id = "cidade",
        Label = "Cidade",
        SearchHandlerName = "SearchCity",
        SelectedDataHandlerName = "SelectedCity",
        Grid = new()
        {
            Id = "cidade",
            Columns =
            [
                new LookupColumnViewModel()
                {
                    Header = "Cidade",
                    Property = "Name"
                },
                new LookupColumnViewModel()
                {
                    Header = "UF",
                    Property = "State"
                }
            ]
        }
    };

    public async Task OnGetAsync(int pageNumber = 1, int pageSize = 10)
    {
        await LoadClubs(pageNumber, pageSize);
    }

    public async Task<IActionResult> OnGetGridAsync(int? pageNumber, int? pageSize)
    {
        await LoadClubs(pageNumber ?? 1, pageSize ?? 10);
        return Partial(PartialViewId.Clubs_Grid, this);
    }

    public async Task<IActionResult> OnGetForm(Guid? id)
    {
        Data = new();
        if (id is not null)
        {
            Club club = await dbContext.Set<Club>().FirstAsync(x => x.Id == id);
            Data = new()
            {
                Id = club.Id,
                Name = club.Name,
                FullName = club.FullName,
                City = club.City,
                State = club.State
            };
        }
        return Partial(PartialViewId.Clubs_Form, this);
    }

    public async Task<IActionResult> OnPostSave()
    {
        ValidationResult validationResult = new Validator().Validate(Data);
        if (!validationResult.IsValid)
        {
            return Partial(PartialViewId.ModalDialog, new ModalDialogViewModel(
                Type: ModalDialogType.Warning,
                Title: "Atenção!",
                Messages: validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        Club club = new(
            Data.Id ?? Guid.CreateVersion7(),
            Data.Name,
            Data.FullName,
            Data.City,
            Data.State
        );

        if (Data.Id is null)
            dbContext.Set<Club>().Add(club);
        else
            dbContext.Attach(club).State = EntityState.Modified;

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClubExists(club.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        Response.Headers["HX-Trigger"] = JsonSerializer.Serialize(new
        {
            updatedClubs = "",
            refreshform = new { id = club.Id }
        });

        return Partial(PartialViewId.ModalDialog, new ModalDialogViewModel(
            Type: ModalDialogType.Success,
            Title: "Sucesso!",
            Messages: ["Clube salvo com sucesso."]));
    }

    public IActionResult OnGetDeleteConfirmation(Guid id)
    {
        return Partial(PartialViewId.ModalDialog, new ModalDialogViewModel(
            ModalDialogType.Question,
            "Excluir Clube",
            ["Tem certeza que deseja excluir este clube?"],
            PostBackUrl: $"/Clubs?handler=Delete&id={id}"
        ));
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        Club? club = await dbContext.Set<Club>().FindAsync(id);

        if (club is null)
        {
            return NotFound();
        }

        dbContext.Set<Club>().Remove(club);
        await dbContext.SaveChangesAsync();

        Response.Headers.Append("HX-Trigger", "updatedClubs, closemodalcontainer");

        return Partial(PartialViewId.ModalDialog, new ModalDialogViewModel(
            ModalDialogType.Success,
            "Sucesso!",
            ["Clube excluído com sucesso."]
        ));
    }

    public async Task<IActionResult> OnPostSearchCity(string searchQuery)
    {
        var results = dbContext.Set<City>()
            .Where(c => c.Name.Contains(searchQuery))
            .ToList();

        CityLookup.Grid.Items = results;
        return Partial(PartialViewId.Lookup_LookupGrid, CityLookup.Grid);
    }

    public async Task<IActionResult> OnPostSelectedCity(string selectedData)
    {
        City? city = JsonSerializer.Deserialize<City>(selectedData);
        if (city is null)
            return StatusCode(500);

        var data = new
        {
            state = city.State
        };

        Response.Headers.Append("HX-Trigger", JsonSerializer.Serialize(new
        {
            fillcitydata = data
        }));

        return StatusCode(204);
    }

    private bool ClubExists(Guid id)
    {
        return dbContext.Set<Club>().Any(e => e.Id == id);
    }

    private async Task LoadClubs(int pageNumber = 1, int pageSize = 10)
    {
        IOrderedQueryable<Club> query = dbContext
            .Set<Club>()
            .AsNoTracking()
            .OrderBy(p => p.Name);

        Pagination = new()
        {
            CurrentPageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = await query.CountAsync(),
        };

        Clubs = await query
            .Skip((Pagination.PageNumber - 1) * Pagination.PageSize)
            .Take(Pagination.PageSize)
            .ToListAsync();
    }

    public class ViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }

    public class Validator : AbstractValidator<ViewModel>
    {
        public Validator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(p => p.FullName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(p => p.City)
                .NotEmpty()
                .MaximumLength(72);

            RuleFor(p => p.State)
                .NotEmpty()
                .MaximumLength(2);
        }
    }
}