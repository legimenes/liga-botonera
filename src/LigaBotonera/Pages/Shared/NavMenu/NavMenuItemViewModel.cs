namespace LigaBotonera.Pages.Shared.NavMenu;

public class NavMenuItemViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Path { get; set; }
    public List<NavMenuItemViewModel> SubMenus { get; set; } = [];
}