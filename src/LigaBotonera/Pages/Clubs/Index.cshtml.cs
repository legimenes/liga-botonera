using FluentValidation;
using FluentValidation.Results;
using LigaBotonera.Entities;
using LigaBotonera.Pages.Shared.ModalDialog;
using LigaBotonera.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace LigaBotonera.Pages.Clubs;
public class Index(ApplicationDbContext dbContext) : PageModel
{
    public IList<Club> Clubs { get; set; } = [];

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public int StartItem => TotalRecords == 0 ? 0 : ((PageNumber - 1) * PageSize) + 1;
    public int EndItem => Math.Min(PageNumber * PageSize, TotalRecords);

    public async Task OnGetAsync(int pageNumber = 1, int pageSize = 10)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;

        IOrderedQueryable<Club> query = dbContext
            .Set<Club>()
            .AsNoTracking()
            .OrderBy(p => p.Name);

        TotalRecords = await query.CountAsync();
        TotalPages = (int)Math.Ceiling(TotalRecords / (double)PageSize);

        if (PageNumber > TotalPages && TotalPages > 0)
            PageNumber = TotalPages;

        Clubs = await query
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();
    }

    public IActionResult OnGetForm(int? id)
    {
        ViewModel club = new();
        return Partial("Clubs/_Form", club);
    }

    public IActionResult OnPostSave(ViewModel viewModel)
    {
        ValidationResult validationResult = new Validator().Validate(viewModel);
        if (!validationResult.IsValid)
        {
            ModalDialogViewModel modalDialogViewModel = new(
                Type: ModalDialogType.Warning,
                Title: "Atenção!",
                Messages: validationResult.Errors.Select(e => e.ErrorMessage));

            return new PartialViewResult
            {
                ViewName = "ModalDialog/_ModalDialog",
                ViewData = new ViewDataDictionary<ModalDialogViewModel>(ViewData, modalDialogViewModel)
            };

        //    TempData.Set("MessageModalNotify", new MessageModalViewModel
        //    (
        //        Type: MessageModalType.Question,
        //        Title: "Atenção!",
        //        Messages: validationResult.Errors.Select(e => e.ErrorMessage)
        //    ));

        //    return Page();
        }

        //Club club = new(
        //    Guid.NewGuid(),
        //    Data.Name,
        //    Data.FullName,
        //    Data.City,
        //    Data.State
        //);

        //context.Set<Club>().Add(club);
        //await context.SaveChangesAsync();

        return RedirectToPage("Index");
    }

    public record ViewModel
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