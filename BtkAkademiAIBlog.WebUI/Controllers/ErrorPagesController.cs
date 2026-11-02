using Microsoft.AspNetCore.Mvc;

namespace BtkAkademiAIBlog.WebUI.Controllers
{
    public class ErrorPagesController : Controller
    {
        public IActionResult Page404()
        {
            return View();
        }
    }
}
