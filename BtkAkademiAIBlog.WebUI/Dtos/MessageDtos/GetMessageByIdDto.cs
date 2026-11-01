namespace BtkAkademiAIBlog.WebUI.Dtos.MessageDtos
{
    public class GetMessageByIdDto
    {
        public int MessageId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
        public string AIStatus { get; set; }
        public bool IsRead { get; set; }
        public DateTime SendDate { get; set; }
    }
}
