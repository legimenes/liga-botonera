namespace LigaBotonera.ViewComponents.Pagination;

public class PaginationViewModel
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int TotalRecords { get; init; }
    public string PageName { get; init; } = default!;
    public IDictionary<string, string?> RouteValues { get; init; } = new Dictionary<string, string?>();
    public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
    public int StartItem => TotalRecords == 0 ? 0 : ((PageNumber - 1) * PageSize) + 1;
    public int EndItem => Math.Min(PageNumber * PageSize, TotalRecords);
}