using Hangfire;
using HangfireAndMailService.Models;
using HangfireAndMailService.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HangfireAndMailService.Services.Manager
{
    public class AuthManager : IAuthService
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly ILogger<AuthManager> _logger;
        public AuthManager(UserManager<AppUser> userManager, IMailService mailService, ILogger<AuthManager> logger, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _mailService = mailService;
            _logger = logger;
            _signInManager = signInManager;
        }
        public async Task<IdentityResult> AddAsync(RegisterVM authVM)
        {
            var user = new AppUser()
            {
                Email = authVM.Email,
                UserName = authVM.UserName,
                FirstName = authVM.FirstName,
                LastName = authVM.LastName,
                PhoneNumber = authVM.PhoneNumber,
                BirthDate = authVM.BirthDate,
                LockoutEnabled = true
               
            };
            var result = await _userManager.CreateAsync(user, authVM.Password);
            if (result.Succeeded)
            {
                BackgroundJob.Schedule(() => Kilit(authVM.UserName),TimeSpan.FromMinutes(1));
            }
            DateTime time = DateTime.Now;
            string displayName = $"{authVM.FirstName} {authVM.LastName}";
            string message = $"<h1>Hoşgeldin <mark>{displayName}</mark></h1><p>{authVM.Email} adresi ile {time} tarihinde <a href='#'>www.hayalyazili.com</a> sitesine üyelik işlemi gerçekleştirdiniz. </p>";

            //_mailService.WelcomeMail(authVM.Email, message); // Thread Sleep diğer tarafta 10 saniye kilitlenecek bu yüzden işlemler devam edemeyecek

            // Ateşle unut
            string jobId = BackgroundJob.Enqueue(() => _mailService.WelcomeMail(authVM.Email, message));
            

            // İş bittiğinde devam et
            BackgroundJob.ContinueJobWith(jobId, () => MailLogInfo(authVM.Email));
            return result;
        }
        public void MailLogInfo(string mail)
        {
            string message = $"{mail} adresine hoşgeldin mesajı iletilmiştir.";
            _logger.LogInformation(message);
        }
        public void Kilit(string userName)
        {
            _logger.LogInformation("kilit calişti >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<");
            var user = _userManager.FindByNameAsync(userName).Result;
            if (user != null)
            {
                user.LockoutEnabled = false;
                _userManager.UpdateAsync(user);
            }
        }

        public async Task<SignInResult> LoginAsync(LoginVM loginVM)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password,true,false);
            return result;
        }
    }
}
