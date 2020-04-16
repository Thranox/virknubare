using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;

namespace Web.Controllers
{
    public class UserManager : IUserManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public UserEntity GetUser(IIdentity userIdentity)
        {
            var claims = (userIdentity as ClaimsIdentity).Claims;
            var sub = claims.Single(x => x.Type == "sub").Value;
            
            var userEntities = _unitOfWork.Repository.List(new UserBySubSpecification(sub)).SingleOrDefault();

            // TODO Update user entity with claims, as needed.

            return userEntities;
        }
    }
}