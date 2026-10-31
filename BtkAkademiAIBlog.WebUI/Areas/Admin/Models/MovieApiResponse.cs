namespace BtkAkademiAIBlog.WebUI.Areas.Admin.Models
{
    public class MovieApiResponse
    {
        public int page { get; set; }
        public List<MovieDataViewModel> result { get; set; } = new();
    }
}
