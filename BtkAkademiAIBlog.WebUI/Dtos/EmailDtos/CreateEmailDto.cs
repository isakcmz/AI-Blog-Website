namespace BtkAkademiAIBlog.WebUI.Dtos.EmailDtos
{
    public class CreateEmailDto
    {
        public string NameSurname { get; set; }
        public string ReceiverMail { get; set; }
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
    }
}
