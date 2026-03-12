using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LigaBotonera.Pages.Shared.Lookup;

public class LookupHandler
{
    public static async Task<IActionResult> Search(PageModel pageModel, LookupGridViewModel lookupGrid, IEnumerable<object> records)
    {
        bool isSelected = pageModel.Request.Headers["X-ItemSelected"] == "true";
        string itemSelectedEvent = $"lookupitemselected-{lookupGrid.LookupId}";

        if (!records.Any())
        {
            pageModel.Response.Headers.Append("HX-Trigger", itemSelectedEvent);
        }

        if (isSelected && records.Count() == 1)
        {
            string jsonData = JsonSerializer.Serialize(records.First());
            string headerValue = $"{{\"{itemSelectedEvent}\": {jsonData}}}";

            pageModel.Response.Headers.Append("HX-Trigger", headerValue);

            return pageModel.Content("");
        }

        lookupGrid.Items = records;
        return pageModel.Partial(PartialViewId.Lookup_LookupGrid, lookupGrid);
    }
}