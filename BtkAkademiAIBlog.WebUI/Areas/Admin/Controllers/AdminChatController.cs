using Microsoft.AspNetCore.Mvc;

namespace BtkAkademiAIBlog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminChatController : Controller
    {
        public IActionResult SendChatWithAI()
        {
            return View();
        }
    }
}
