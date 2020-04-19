using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IDP
{
    public class UserIdentityDbContext:IdentityDbContext
    {
        public UserIdentityDbContext( DbContextOptions<UserIdentityDbContext> options):base(options)
        {
        }
    }
}