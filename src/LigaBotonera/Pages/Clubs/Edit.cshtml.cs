using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LigaBotonera.Entities;
using LigaBotonera.Persistence;

namespace LigaBotonera.Pages.Clubs
{
    public class EditModel : PageModel
    {
        private readonly LigaBotonera.Persistence.ApplicationDbContext _context;

        public EditModel(LigaBotonera.Persistence.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Club Club { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club =  await _context.Set<Club>().FirstOrDefaultAsync(m => m.Id == id);
            if (club == null)
            {
                return NotFound();
            }
            Club = club;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Club).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClubExists(Club.Id))
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

        private bool ClubExists(Guid id)
        {
            return _context.Set<Club>().Any(e => e.Id == id);
        }
    }
}
