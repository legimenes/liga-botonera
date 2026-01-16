using LigaBotonera.Entities;
using LigaBotonera.Persistence;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LigaBotonera.Pages.Clubs;
public class IndexModel(ApplicationDbContext dbContext) : PageModel
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
}