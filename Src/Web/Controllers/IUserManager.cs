using System.Security.Principal;
using Domain.Entities;

namespace Web.Controllers
{
    public interface IUserManager
    {
        UserEntity GetUser(IIdentity userIdentity);
    }
}