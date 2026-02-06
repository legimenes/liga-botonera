using FluentValidation;
using FluentValidation.Results;
using LigaBotonera.Entities;
using LigaBotonera.Pages.Shared.ModalDialog;
using LigaBotonera.Persistence;
using LigaBotonera.ViewComponents.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LigaBotonera.Pages.Clubs;
public class Index(ApplicationDbContext dbContext) : PageModel
{
    public IList<Club> Clubs { get; set; } = [];

    public PaginationViewModel Pagination { get; set; } = new();

    public async Task OnGetAsync(int pageNumber = 1, int pageSize = 10)
    {
        await LoadClubs(pageNumber, pageSize);
    }

    public async Task<IActionResult> OnGetGridAsync(int? pageNumber, int? pageSize)
    {
        await LoadClubs(pageNumber ?? 1, pageSize ?? 10);
        return Partial("Clubs/_Grid", this);
    }

    public async Task<IActionResult> OnGetForm(Guid? id)
    {
        //ViewModel club = new(null, string.Empty, string.Empty, string.Empty, string.Empty);
        ViewModel viewModel = new();
        if (id is not null)
        {
            Club club = await dbContext.Set<Club>().FirstAsync(x => x.Id == id);
            viewModel = new()
            {
                Id = club.Id,
                Name = club.Name,
                FullName = club.FullName,
                City = club.City,
                State = club.State
            };
        }
        return Partial("Clubs/_Form", viewModel);
    }

    public async Task<IActionResult> OnPostSave(ViewModel viewModel)
    {
        ValidationResult validationResult = new Validator().Validate(viewModel);
        if (!validationResult.IsValid)
        {
            return Partial("ModalDialog/_ModalDialog", new ModalDialogViewModel(
                Type: ModalDialogType.Warning,
                Title: "Atenção!",
                Messages: validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        Club club = new(
            viewModel.Id ?? Guid.CreateVersion7(),
            viewModel.Name,
            viewModel.FullName,
            viewModel.City,
            viewModel.State
        );

        if (viewModel.Id is null)
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

        Response.Headers.Add("HX-Trigger", "updatedClubs");

        return Partial("ModalDialog/_ModalDialog", new ModalDialogViewModel(
            Type: ModalDialogType.Success,
            Title: "Sucesso!",
            Messages: ["Clube salvo com sucesso."]));
    }

    public IActionResult OnGetDeleteConfirmation(Guid id)
    {
        return Partial("ModalDialog/_ModalDialog", new ModalDialogViewModel(
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

        Response.Headers.Add("HX-Trigger", "updatedClubs");

        return Partial("ModalDialog/_ModalDialog", new ModalDialogViewModel(
            ModalDialogType.Success,
            "Sucesso!",
            ["Clube excluído com sucesso."]
        ));
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
    //public record ViewModel(Guid? Id, string Name, string FullName, string City, string State);

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