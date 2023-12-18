using HangfireAndMailService.Models;
using HangfireAndMailService.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Mail;

namespace HangfireAndMailService.Services.Manager
{
    public class MailManager : IMailService
    {
        public readonly UserManager<AppUser> _userManager;
        private readonly ILogger<MailManager> _logger;
        public MailManager(UserManager<AppUser> userManager, ILogger<MailManager> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public bool SendMail(MailVM mail)
        {
            try
            {
                MailMessage mailMessage = new MailMessage()
                {
                    Subject = mail.Subject,
                    Body = mail.SendMessage,
                    IsBodyHtml = true,
                    From = new MailAddress(mail.SendMail)
                };
                foreach (var item in mail.MailTo)
                {
                    mailMessage.To.Add(item);
                }

                SmtpClient smtpClient = new SmtpClient()
                {
                    Credentials = new NetworkCredential(mail.SendMail, mail.SendPassword),
                    EnableSsl = true,
                    Port = 587,
                    Host = "smtp-mail.outlook.com"
                };

                smtpClient.Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void BirthdayMail()
        {
           
            var users = _userManager.Users.Where(x => x.BirthDate.Day == DateTime.Now.Day && x.BirthDate.Month == DateTime.Now.Month).ToList();
            MailVM mailVM = new MailVM()
            {
                SendDisplayName = "X Firma",
                Subject = "Doğum Günü",
                SendMail = "webtest0@hotmail.com",
                SendPassword = "webuygulama01",
                SendMessage = $"{DateTime.Now}<h1>İyiki doğdun...<3</h1>",
                MailTo = new List<MailAddress>()
            };
            foreach (var user in users) mailVM.MailTo.Add(new MailAddress(user.Email));
            SendMail(mailVM);
            string logInfo = $"{DateTime.Now} tarihinde {users.Count} kişinin doğum günü kutlandı...";
            _logger.LogInformation(logInfo);
        }

        public void WelcomeMail(string email,string message)
        {
            Thread.Sleep(8000);

            var users = _userManager.Users.Where(x => x.BirthDate.Day == DateTime.Now.Day && x.BirthDate.Month == DateTime.Now.Month).ToList();
            MailVM mailVM = new MailVM()
            {
                SendDisplayName = "Hayal Proje",
                Subject = "Kayıt Bilgisi",
                SendMail = "webtest0@hotmail.com",
                SendPassword = "webuygulama01",
                SendMessage = message
            };
            foreach (var user in users) mailVM.MailTo.Add(new MailAddress(email));
            SendMail(mailVM);
        }
    }
}
