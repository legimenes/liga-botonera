namespace LigaBotonera.Pages.Shared.Lookup2;

public class LookupGridViewModel
{
    public Func<object, object> IdSelector { get; set; } = null!;
    public Func<object, string> NameSelector { get; set; } = null!;
    public List<LookupGridColumnViewModel> Columns { get; set; } = [];
    public List<object> Items { get; set; } = [];
}