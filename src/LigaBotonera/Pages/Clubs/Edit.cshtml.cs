using FluentValidation;
using FluentValidation.Results;
using LigaBotonera.Entities;
using LigaBotonera.Extensions;
using LigaBotonera.Persistence;
using LigaBotonera.ViewComponents.MessageModal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LigaBotonera.Pages.Clubs;

public class Edit(ApplicationDbContext context) : PageModel
{
    [BindProperty]
    public ViewModel Data { get; set; } = default!;

    public MessageModalViewModel? MessageModal { get; set; }

    public record ViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }

    public class Validator : AbstractValidator<ViewModel>
    {
        public Validator()
        {
            RuleFor(p => p.Id)
                .NotEmpty();

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

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Club? club =  await context.Set<Club>().FirstOrDefaultAsync(m => m.Id == id);
        if (club == null)
        {
            return NotFound();
        }

        ViewModel viewModel = new()
        {
            Id = club.Id,
            Name = club.Name,
            FullName = club.FullName,
            City = club.City,
            State = club.State
        };
        Data = viewModel;

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        ValidationResult validationResult = new Validator().Validate(Data);
        if (!validationResult.IsValid)
        {
            TempData.Set("MessageModalNotify", new MessageModalViewModel
            (
                Type: MessageModalType.Question,
                Title: "Atenção!",
                Messages: validationResult.Errors.Select(e => e.ErrorMessage)
            ));

            return Page();
        }

        Club club = new(
            Data.Id,
            Data.Name,
            Data.FullName,
            Data.City,
            Data.State
        );
        context.Attach(club).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
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

        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        var club = await context.Set<Club>().FindAsync(id);

        if (club == null)
        {
            return NotFound();
        }

        context.Set<Club>().Remove(club);
        await context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private bool ClubExists(Guid id)
    {
        return context.Set<Club>().Any(e => e.Id == id);
    }
}
