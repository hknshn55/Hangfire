using HangfireAndMailService.Models.ViewModels;

namespace HangfireAndMailService.Services
{
    public interface IMailService
    {
       bool SendMail(MailVM mail);
        void BirthdayMail();
        void WelcomeMail(string email, string message);
    }
}
