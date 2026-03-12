namespace LigaBotonera.Pages.Shared.Lookup;

public class LookupGridViewModel
{
    public string LookupId { get; set; } = string.Empty;
    public List<LookupColumnViewModel> Columns { get; set; } = [];
    public IEnumerable<object> Items { get; set; } = [];
}
