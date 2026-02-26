namespace LigaBotonera.Pages.Shared.Lookup;

public class LookupGridViewModel
{
    public string Id { get; set; } = string.Empty;
    public List<LookupColumnViewModel> Columns { get; set; } = [];
    public IEnumerable<object> Items { get; set; } = [];
}
