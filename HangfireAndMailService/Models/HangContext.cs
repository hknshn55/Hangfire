using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HangfireAndMailService.Models
{
    public class HangContext : IdentityDbContext<AppUser,AppRole,int>
    {
        public HangContext(DbContextOptions<HangContext> options):base(options)
        {
              
        }
    }
}
