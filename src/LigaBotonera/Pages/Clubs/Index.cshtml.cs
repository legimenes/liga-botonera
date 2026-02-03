using System;
using FluentValidation;
using FluentValidation.Results;
using LigaBotonera.Entities;
using LigaBotonera.Pages.Shared.ModalDialog;
using LigaBotonera.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

    public async Task<IActionResult> OnGetGridAsync(int? pageNumber)
    {
        PageNumber = 1;
        PageSize = 10;

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
            Type: ModalDialogType.Warning,
            Title: "Sucesso!",
            Messages: ["Clube criado com sucesso."]));
        //return RedirectToPage("Index");
    }

    private bool ClubExists(Guid id)
    {
        return dbContext.Set<Club>().Any(e => e.Id == id);
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