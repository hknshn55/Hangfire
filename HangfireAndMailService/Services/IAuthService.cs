using HangfireAndMailService.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace HangfireAndMailService.Services
{
    public interface IAuthService
    {
       Task< IdentityResult> AddAsync(RegisterVM authVM);
        Task<SignInResult> LoginAsync(LoginVM loginVM);

    }
}
