namespace LigaBotonera.Pages.Shared.Lookup;

public class LookupViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string DisplayProperty { get; set; } = string.Empty;
    public string DataIdProperty { get; set; } = string.Empty;
    public string SearchHandlerName { get; set; } = string.Empty;
    public string SelectedDataHandlerName { get; set; } = string.Empty;
    public string InitialId { get; set; } = string.Empty;
    public string InitialValue { get; set; } = string.Empty;
    public LookupGridViewModel Grid
    {
        get;
        set
        {
            field = value;
            field.LookupId = Id;
        }
    } = new();

    public void SetInitialValue(string id, string value)
    {
        InitialId = id;
        InitialValue = value;
    }
}