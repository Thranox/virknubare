using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.AspNetCore.Http;
using SharedWouldBeNugets;

namespace API.Shared.Services
{
    public class SubManagementService : ISubManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PolApiContext> GetPolApiContext(HttpContext httpContext)
        {
            var fullUrl = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(httpContext.Request);

            var userIdentity =httpContext.User.Identity;
            var claims = (userIdentity as ClaimsIdentity).Claims;
            var sub = claims.Single(x => x.Type == ImproventoGlobals.ImproventoSubClaimName).Value;

            var userEntity = _unitOfWork.Repository.List(new UserBySub(sub)).SingleOrDefault();
            if (userEntity == null)
            {
                userEntity = new UserEntity(claims.Single(x => x.Type == ClaimTypes.Name).Value, sub);
                _unitOfWork.Repository.Add(userEntity);
            }
            else
            {
                userEntity.UpdateWithClaims(claims);
            }
            await _unitOfWork.CommitAsync();

            return new PolApiContext(userEntity, fullUrl);
        }
    }
}