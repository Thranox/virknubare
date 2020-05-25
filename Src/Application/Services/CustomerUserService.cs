using System;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Specifications;

namespace Application.Services
{
    public class CustomerUserService : ICustomerUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerUserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomerUserGetResponse> GetAsync(PolApiContext sub, Guid customerId)
        {
            await Task.CompletedTask;

            var customer = _unitOfWork.Repository.List(new CustomerById(customerId)).SingleOrDefault() ??
                           throw new ArgumentException(nameof(customerId));
            
            return new CustomerUserGetResponse
            {
                Users = customer
                    .CustomerUserPermissions
                    .Select(x => _mapper.Map<UserPermissionDto>(x))
                    .ToArray()
            };
        }

        public async Task<CustomerInvitationsPostResponse> CreateInvitationsAsync(PolApiContext sub, Guid customerId,
            CustomerInvitationsPostDto customerInvitationsPostDto)
        {
            var customer = _unitOfWork.Repository.List(new CustomerById(customerId)).SingleOrDefault() ??
                           throw new ArgumentException(nameof(customerId));

            foreach (var email in customerInvitationsPostDto.Emails)
            {
                customer.AddInvitation(email);
            }

            await _unitOfWork.CommitAsync();

            return new CustomerInvitationsPostResponse();
        }
    }
}