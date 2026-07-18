namespace LigaBotonera.Pages.Shared.Lookup2;

public class LookupViewModel
{
    public string Id { get; set; } = "lookup_" + Guid.NewGuid().ToString("N")[..8];
    public string Label { get; set; } = string.Empty;
    public string QueryHandlerName { get; set; } = string.Empty;
    public LookupGridViewModel Grid { get; set; } = new();
}
