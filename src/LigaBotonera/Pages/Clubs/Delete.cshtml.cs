using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LigaBotonera.Entities;
using LigaBotonera.Persistence;

namespace LigaBotonera.Pages.Clubs
{
    public class DeleteModel : PageModel
    {
        private readonly LigaBotonera.Persistence.ApplicationDbContext _context;

        public DeleteModel(LigaBotonera.Persistence.ApplicationDbContext context)
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

            var club = await _context.Set<Club>().FirstOrDefaultAsync(m => m.Id == id);

            if (club is not null)
            {
                Club = club;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.Set<Club>().FindAsync(id);
            if (club != null)
            {
                Club = club;
                _context.Set<Club>().Remove(Club);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
