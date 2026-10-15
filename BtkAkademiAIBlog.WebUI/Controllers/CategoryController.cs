using Microsoft.AspNetCore.Mvc;

namespace BtkAkademiAIBlog.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult CategoryList()
        {
            return View();
        }
    }
}
