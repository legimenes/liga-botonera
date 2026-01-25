using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Teste.Pages;

public class IndexModel : PageModel
{
    public void OnGet()
    {
    }

    public IActionResult OnGetModalContent()
    {
        return Partial("_ContactFormPartial");
    }
}
