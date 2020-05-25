using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Domain.Interfaces;
using Domain.SharedKernel;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class UserEntity : BaseEntity, IMessageReceiver
    {
        private UserEntity()
        {
            FlowStepUserPermissions=new List<FlowStepUserPermissionEntity>();
            CustomerUserPermissions=new List<CustomerUserPermissionEntity>();
            TravelExpenses=new List<TravelExpenseEntity>();
        }

        public UserEntity(string name, string subject) : this()
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Subject = subject ?? throw new ArgumentNullException(nameof(subject));
        }

        public string Name { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public ICollection<FlowStepUserPermissionEntity> FlowStepUserPermissions { get; set; }
        public ICollection<CustomerUserPermissionEntity>  CustomerUserPermissions { get; set; }
        public ICollection<TravelExpenseEntity> TravelExpenses { get; set; }

        public bool IsUserAdminForCustomer(Guid customerId)
        {
            return CustomerUserPermissions
                .Any(x => x.CustomerId == customerId && x.UserStatus == UserStatus.UserAdministrator);
        }

        public void Enrich(Dictionary<string, string> messageValues)
        {
            messageValues.Add(KeyMessagesConst.UserName, Name);
        }

        public void UpdateWithClaims(IEnumerable<Claim> claims)
        {
            var emailClaim = claims.FirstOrDefault(x => x.Type == "email");
            if (emailClaim != null) Email = emailClaim.Value;

            var nameClaim = claims.FirstOrDefault(x => x.Type == "name");
            if (nameClaim != null) Name= nameClaim.Value;
        }
    }
}