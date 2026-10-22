namespace BtkAkademiAIBlog.WebUI.Dtos.ContactDtos
{
    public class UpdateContactDto
    {
        public int ContactId { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Address { get; set; }
        public string MapLocation { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
    }
}
