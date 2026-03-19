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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LigaBotonera.Pages.Clubs;
public class Index(ApplicationDbContext dbContext) : PageModel
{
    [BindProperty]
    public ViewModel Data { get; set; } = new();

    public IList<ViewModel> Clubs { get; set; } = [];

    public IList<SelectListItem> StateOptions { get; set; } = [];

    public PaginationViewModel Pagination { get; set; } = new();

    public LookupViewModel CityLookup = new()
    {
        Id = "city",
        Label = "Cidade",
        DisplayProperty = "Name",
        DataIdProperty = "Id",
        SearchHandlerName = "SearchCity",
        SelectedDataHandlerName = "SelectedCity",
        Grid = new()
        {
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
        await LoadStateOptions();
        Data = new();
        if (id is not null)
        {
            Data = await dbContext.Set<Club>()
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(club => new ViewModel
                {
                    Id = club.Id,
                    Name = club.Name,
                    FullName = club.FullName,
                    City = club.City.Name,
                    CityId = club.CityId,
                    State = club.City.State.Name,
                    StateId = club.City.State.Id
                })
                .SingleAsync();
        }
        CityLookup.SetInitialValue(Data.CityId.ToString(), Data.City);
        return Partial(PartialViewId.Clubs_Form, this);
    }

    public async Task<IActionResult> OnPostSave()
    {
        ValidationResult validationResult = new Validator().Validate(Data);
        if (!validationResult.IsValid)
        {
            return Partial(PartialViewId.ModalDialog, new ModalDialogViewModel(
                Type: ModalDialogType.Warning,
                Messages: validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        Club club = new(
            id: Data.Id ?? Guid.CreateVersion7(),
            name: Data.Name,
            fullName: Data.FullName,
            cityId: Data.CityId
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
            Messages: ["Clube salvo com sucesso."]));
    }

    public IActionResult OnGetDeleteConfirmation(Guid id)
    {
        return Partial(PartialViewId.ModalDialog, new ModalDialogViewModel(
            Type: ModalDialogType.Question,
            Title: "Excluir Clube",
            Messages: ["Tem certeza que deseja excluir este clube?"],
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
            Type: ModalDialogType.Success,
            Title: "Sucesso!",
            Messages: ["Clube excluído com sucesso."]
        ));
    }

    public async Task<IActionResult> OnPostSearchCity(string searchQuery)
    {
        IEnumerable<CityViewModel> records = dbContext.Set<City>()
            .Select(p => new CityViewModel
            {
                Id = p.Id,
                Name = p.Name,
                StateId = p.StateId,
                State = p.State.Name
            })
            .Where(c => c.Name.Contains(searchQuery));

        return await LookupHandler.Search(
            pageModel: this,
            lookupGrid: CityLookup.Grid,
            records: records);
    }

    public async Task<IActionResult> OnPostSelectedCity(string selectedData)
    {
        CityViewModel? city = null;
        if (selectedData != "[]")
            city = JsonSerializer.Deserialize<CityViewModel>(selectedData);

        var data = city is null ? null : new
        {
            stateId = city.StateId
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

    private async Task LoadStateOptions()
    {
        StateOptions = await dbContext.Set<State>()
            .OrderBy(p => p.Name)
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            })
            .ToListAsync();
    }

    private async Task LoadClubs(int pageNumber = 1, int pageSize = 10)
    {
        IQueryable<ViewModel> query = dbContext.Set<Club>()
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(club => new ViewModel
            {
                Id = club.Id,
                Name = club.Name,
                FullName = club.FullName,
                City = club.City.Name,
                CityId = club.CityId,
                State = club.City.State.Name,
                StateId = club.City.State.Id
            });

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
        public int CityId { get; set; }
        public string State { get; set; } = string.Empty;
        public int StateId { get; set; }
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

    public class CityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StateId { get; set; }
        public string State { get; set; } = string.Empty;
    }
}