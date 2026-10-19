using BtkAkademiAIBlog.WebUI.Areas.Admin.Models;
using BtkAkademiAIBlog.WebUI.Dtos.EmailDtos;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace BtkAkademiAIBlog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminEmailController : Controller
    {
        private readonly IConfiguration _configuration;

        public AdminEmailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }




        [HttpGet]
        public IActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail(CreateEmailDto createEmailDto)
        {
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderName = _configuration["EmailSettings:SenderName"];
            var appPassword = _configuration["EmailSettings:AppPassword"];


            MimeMessage mimeMessage = new MimeMessage();

            MailboxAddress mailboxAddressFrom = new MailboxAddress(senderName, senderEmail);
            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", createEmailDto.ReceiverMail);
            mimeMessage.To.Add(mailboxAddressTo);

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = createEmailDto.MessageDetail;
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            mimeMessage.Subject = createEmailDto.Subject;

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate(senderEmail, appPassword);
            client.Send(mimeMessage);
            client.Disconnect(true);

            return RedirectToAction("Index");

        }


    }
}
