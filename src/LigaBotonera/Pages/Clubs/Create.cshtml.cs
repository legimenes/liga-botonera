using FluentValidation;
using FluentValidation.Results;
using LigaBotonera.Entities;
using LigaBotonera.Extensions;
using LigaBotonera.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LigaBotonera.Pages.Clubs;

public class Create(ApplicationDbContext context) : PageModel
{
    [BindProperty]
    public ViewModel Data { get; set; } = default!;

    //public MessageModalViewModel? MessageModal { get; set; }

    public record ViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }

    public class Validator: AbstractValidator<ViewModel>
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

    public async Task<IActionResult> OnPostAsync()
    {
        ValidationResult validationResult = new Validator().Validate(Data);
        if (!validationResult.IsValid)
        {
            //TempData.Set("MessageModalNotify", new MessageModalViewModel
            //(
            //    Type: MessageModalType.Question,
            //    Title: "Atenção!",
            //    Messages: validationResult.Errors.Select(e => e.ErrorMessage)
            //));

            return Page();
        }

        Club club = new(
            Guid.NewGuid(),
            Data.Name,
            Data.FullName,
            Data.City,
            Data.State
        );

        context.Set<Club>().Add(club);
        await context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}