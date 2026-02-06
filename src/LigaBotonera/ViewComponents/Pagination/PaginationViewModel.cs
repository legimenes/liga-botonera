namespace LigaBotonera.ViewComponents.Pagination;

public class PaginationViewModel
{
    public int CurrentPageNumber { get; init; } = 1;
    public int PageNumber
    {
        get
        {
            int pageNumber = CurrentPageNumber;
            if (pageNumber > TotalPages && TotalPages > 0)
                pageNumber = TotalPages;

            if (pageNumber < 1)
                pageNumber = 1;

            return pageNumber;
        }
    }  
    public int PageSize { get; init; } = 10;
    public int TotalRecords { get; init; }
    public string PageName { get; init; } = default!;
    public IDictionary<string, string?> RouteValues { get; init; } = new Dictionary<string, string?>();
    public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
    public int StartItem => TotalRecords == 0 ? 0 : ((PageNumber - 1) * PageSize) + 1;
    public int EndItem => Math.Min(PageNumber * PageSize, TotalRecords);
}

//await Component.InvokeAsync("Pagination", new
//{
//    pageNumber = Model.PageNumber,
//    pageSize = Model.PageSize,
//    totalRecords = Model.TotalRecords,
//    pageName = "/Clubs/Index",
//     routeValues = new Dictionary<string, string?>
//     {
//         ["search"] = Model.SearchTerm,
//         ["status"] = Model.Status
//     }
//})