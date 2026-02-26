using Microsoft.AspNetCore.Mvc;

namespace LigaBotonera.ViewComponents.Pagination;
public class PaginationViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(
        int pageNumber,
        int pageSize,
        int totalRecords,
        string pageName,
        IDictionary<string, string?>? routeValues = null)
    {
        PaginationViewModel model = new()
        {
            CurrentPageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            PageName = pageName,
            RouteValues = routeValues ?? new Dictionary<string, string?>()
        };

        return View(model);
    }
}