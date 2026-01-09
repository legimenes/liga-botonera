using Microsoft.AspNetCore.Mvc;

namespace LigaBotonera.ViewComponents.Pagination;
public class PaginationViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(
        int pageNumber,
        int pageSize,
        int totalPages,
        int totalRecords,
        string pageName,
        IDictionary<string, string?>? routeValues = null)
    {
        var model = new PaginationViewModel
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalRecords = totalRecords,
            PageName = pageName,
            RouteValues = routeValues ?? new Dictionary<string, string?>()
        };

        return View(model);
    }
}