namespace LigaBotonera.Pages.Shared.Lookup;

public class LookupViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string HandlerName { get; set; } = string.Empty;
    public List<LookupColumnViewModel> Columns { get; set; } = [];
}