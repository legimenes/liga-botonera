using Microsoft.AspNetCore.Mvc;

namespace LigaBotonera.ViewComponents.ModalContainer;

public class ModalContainerViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}