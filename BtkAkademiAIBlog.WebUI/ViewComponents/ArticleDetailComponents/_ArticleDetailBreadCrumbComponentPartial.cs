using Microsoft.AspNetCore.Mvc;

namespace BtkAkademiAIBlog.WebUI.ViewComponents.ArticleDetailComponents
{
    public class _ArticleDetailBreadCrumbComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke(int id)
        {

            return View();
        }
    }
}
