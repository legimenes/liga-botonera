namespace LigaBotonera.Pages.Shared.Lookup;

public class LookupGridColumnViewModel
{
    public string Header { get; set; } = string.Empty;
    public Func<object, string> ValueSelector { get; set; } = null!;
    public string? CssClass { get; set; }
}