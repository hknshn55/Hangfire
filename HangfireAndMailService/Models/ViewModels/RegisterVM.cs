namespace HangfireAndMailService.Models.ViewModels
{
    public class RegisterVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserName {  get; set; }
        public DateTime BirthDate { get; set; }
    }
}
