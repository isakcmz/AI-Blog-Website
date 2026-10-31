namespace BtkAkademiAIBlog.WebUI.Areas.Admin.Models
{
    public class MovieListPageViewModel
    {
        public List<MovieDataViewModel> Movies { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; } = 10; // 1000 film / 100
        public string ErrorMessage { get; set; }
    }
}
