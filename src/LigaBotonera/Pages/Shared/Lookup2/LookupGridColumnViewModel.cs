namespace LigaBotonera.Pages.Shared.Lookup2;

public class LookupGridColumnViewModel
{
    public string Header { get; set; } = string.Empty;
    public Func<object, string> ValueSelector { get; set; } = null!;
    public string? CssClass { get; set; }
}