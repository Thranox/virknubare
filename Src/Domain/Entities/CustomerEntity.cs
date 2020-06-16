using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.SharedKernel;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class CustomerEntity : BaseEntity, IMessageValueEnricher
    {
        private CustomerEntity()
        {
            FlowSteps = new List<FlowStepEntity>();
            CustomerUserPermissions = new List<CustomerUserPermissionEntity>();
            TravelExpenses = new List<TravelExpenseEntity>();
            Invitations = new List<InvitationEntity>();
        }

        public CustomerEntity(string name) : this()
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public ICollection<TravelExpenseEntity> TravelExpenses { get; }
        public ICollection<FlowStepEntity> FlowSteps { get; }
        public ICollection<CustomerUserPermissionEntity> CustomerUserPermissions { get; }
        public string Name { get; set; }

        public ICollection<InvitationEntity> Invitations { get; }
        public string FtpIdentifier { get; set; } = "SL";

        public void AddUser(UserEntity userEntity, UserStatus userStatus)
        {
            if (userEntity == null)
                throw new ArgumentNullException(nameof(userEntity), "User to be added can't be null");

            if (CustomerUserPermissions.Any(x => x.CustomerId == userEntity.Id))
                throw new BusinessRuleViolationException(userEntity.Id,
                    "User " + userEntity.Id + " already exists for Customer " + Id);

            CustomerUserPermissions.Add(new CustomerUserPermissionEntity {User = userEntity, UserStatus = userStatus});
        }

        public IEnumerable<UserEntity> GetUsersAbleToProcess(StageEntity stageEntity)
        {
            return FlowSteps
                .Where(xxx => xxx.From == stageEntity)
                .SelectMany(x => x.FlowStepUserPermissions)
                .Select(xx => xx.User)
                .Distinct()
                .ToArray();
        }

        public void SetUserStatus(UserEntity userEntity, UserStatus userStatus)
        {
            var customerUserPermissionEntity = CustomerUserPermissions.SingleOrDefault(x => x.UserId == userEntity.Id);

            if (customerUserPermissionEntity == null)
            {
                customerUserPermissionEntity = new CustomerUserPermissionEntity
                    {User = userEntity, UserStatus = userStatus};
                CustomerUserPermissions.Add(customerUserPermissionEntity);
            }
            else
                customerUserPermissionEntity.UserStatus = userStatus;
        }

        public void AddInvitation(string email, IMessageValueEnricher[] messageValueEnrichers)
        {
            var invitationEntity = new InvitationEntity(email);
            invitationEntity.Events.Add(new InvitationAddedDomainEvent(this, invitationEntity,messageValueEnrichers));
            Invitations.Add(invitationEntity);
        }

        public void Enrich(Dictionary<string, string> messageValues)
        {
            messageValues.Add(KeyMessagesConst.CustomerName, Name);
        }
    }
}