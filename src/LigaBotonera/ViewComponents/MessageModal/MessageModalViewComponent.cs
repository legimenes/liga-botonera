using LigaBotonera.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LigaBotonera.ViewComponents.MessageModal;
public class MessageModalViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(MessageModalViewModel? messageModal = null)
    {
        MessageModalViewModel data = messageModal ?? TempData.Get<MessageModalViewModel>("MessageModalNotify");
        return View(data);
    }
}