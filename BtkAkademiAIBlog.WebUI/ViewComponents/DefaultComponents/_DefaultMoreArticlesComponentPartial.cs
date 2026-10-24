using Microsoft.AspNetCore.Mvc;

namespace BtkAkademiAIBlog.WebUI.ViewComponents.DefaultComponents
{
    public class _DefaultMoreArticlesComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
