namespace BtkAkademiAIBlog.WebUI.Dtos.CommentDtos
{
    public class GetLast3CommentWithUserDto
    {
        public int CommentId { get; set; }
        public string AppUserId { get; set; }
        public DateTime CommentDate { get; set; }
        public string CommentDetail { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserImageUrl { get; set; }
        public int ArticleId { get; set; } 
    }
}
