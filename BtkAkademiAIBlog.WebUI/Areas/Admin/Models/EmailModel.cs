namespace BtkAkademiAIBlog.WebUI.Areas.Admin.Models
{
    public class EmailModel
    {
        public string SenderNameSurname { get; set; }
        public string SenderEmail { get; set; }
        public string MessageSubject { get; set; }
        public string MessageDetail { get; set; }
        public DateTime SendDate { get; set; }
        public string Status { get; set; }

    }
}
