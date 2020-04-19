using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IDP
{
    public class UserIdentityDbContext:IdentityDbContext<ApplicationUser>
    {
        public UserIdentityDbContext( DbContextOptions<UserIdentityDbContext> options):base(options)
        {
        }
    }

    public class ApplicationUser : IdentityUser
    {
        public string SubjectId { get; set; }
    }
}