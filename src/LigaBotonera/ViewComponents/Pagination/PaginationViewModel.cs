public class PaginationViewModel
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public int StartItem => TotalRecords == 0 ? 0 : ((PageNumber - 1) * PageSize) + 1;
    public int EndItem => Math.Min(PageNumber * PageSize, TotalRecords);

    public string PageName { get; init; } = default!;
    public IDictionary<string, string?> RouteValues { get; init; } = new Dictionary<string, string?>();
}