namespace LigaBotonera.Pages.Shared.Lookup2;

public class LookupViewModel
{
    public string Id { get; } = "lookup_" + Guid.NewGuid().ToString("N")[..8];
    public string Label { get; set; } = string.Empty;
    public string QueryHandlerName { get; set; } = string.Empty;
    public string? InitialId { get; set; }
    public string? InitialValue { get; set; }
    public LookupGridViewModel Grid { get; set; } = new();

    public void SetInitialValue(string id, string value)
    {
        InitialId = id;
        InitialValue = value;
    }
}
