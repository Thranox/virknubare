using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Domain.ValueObjects;

namespace Application.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvitationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AcceptWaitingInvitations(PolApiContext polApiContext)
        {
            var invitationEntities = _unitOfWork.Repository.List(new InvitationsByEmail(polApiContext.CallingUser.Email));
            foreach (var invitationEntity in invitationEntities)
            {
                var customerUserPermissionEntities = invitationEntity.Customer.CustomerUserPermissions.SingleOrDefault(x=>x.UserId==polApiContext.CallingUser.Id);
                if (customerUserPermissionEntities == null)
                {
                    customerUserPermissionEntities = new CustomerUserPermissionEntity(){Customer = invitationEntity.Customer, User = polApiContext.CallingUser};
                    invitationEntity.Customer.CustomerUserPermissions.Add(customerUserPermissionEntities);
                }

                customerUserPermissionEntities.UserStatus = UserStatus.Registered;
                invitationEntity.InvitationState = InvitationState.Used;
            }

            await _unitOfWork.CommitAsync();

            return invitationEntities.Count;
        }
    }
}