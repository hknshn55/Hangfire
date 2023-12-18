using System.Net.Mail;

namespace HangfireAndMailService.Models.ViewModels
{
    public class MailVM
    {
        public MailVM()
        {
            MailTo = new List<MailAddress>();
        }
        public string SendDisplayName { get; set; }
        public string SendMail { get; set; }
        public string SendPassword { get; set; }
        public string SendMessage { get; set; }
        public string Subject { get; set; }
        public List<MailAddress> MailTo { get; set; }
    }
}
