﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;
using SharedWouldBeNugets;

namespace API.Shared.Services
{
    public class SubManagementService : ISubManagementService
    {
        private static PolSystem[] Systems = new[]
        {
            new PolSystem("https://localhost:44348/", "https://localhost:44324/"), 
            new PolSystem("https://api.andersathome.dk/polapi/", "https://andersathome.dk/"),
            new PolSystem("https://apidev.politikerafregning.dk/polapi/", "https://dev.politikerafregning.dk/"),
            new PolSystem("https://api.politikerafregning.dk/polapi/", "https://politikerafregning.dk/"),
        };

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public SubManagementService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<PolApiContext> GetPolApiContext(HttpContext httpContext)
        {
            var fullUrl = UriHelper.GetDisplayUrl(httpContext.Request);

            var system = Systems.FirstOrDefault(x => fullUrl.Contains(x.ApiUrl));

            var userIdentity =httpContext.User.Identity;
            var claims = (userIdentity as ClaimsIdentity).Claims.ToArray();
            var improventoSubClaim = claims.SingleOrDefault(x => x.Type == ImproventoGlobals.ImproventoSubClaimName);
            if (improventoSubClaim == null)
            {
                _logger.Error("User has no improvento sub claim");
            }

            var sub = improventoSubClaim.Value;

            var userEntity = _unitOfWork.Repository.List(new UserBySub(sub)).SingleOrDefault();
            if (userEntity == null)
            {
                userEntity = new UserEntity(claims.Single(x => x.Type == "name").Value, sub);
                _unitOfWork.Repository.Add(userEntity);
            }

            userEntity.UpdateWithClaims(claims);

            await _unitOfWork.CommitAsync();

            return new PolApiContext(userEntity, fullUrl, system);
        }
    }
}