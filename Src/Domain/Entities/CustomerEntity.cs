using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.SharedKernel;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class CustomerEntity : BaseEntity
    {
        private CustomerEntity()
        {
            FlowSteps = new List<FlowStepEntity>();
            CustomerUserPermissions = new List<CustomerUserPermissionEntity>();
            TravelExpenses = new List<TravelExpenseEntity>();
        }

        public CustomerEntity(string name) : this()
        {
            Name = name ?? throw new ArgumentNullException(nameof(name)); 
        }

        public ICollection<TravelExpenseEntity> TravelExpenses { get; }
        public ICollection<FlowStepEntity> FlowSteps { get; }
        public ICollection<CustomerUserPermissionEntity> CustomerUserPermissions  { get; }
        public string Name { get; set; }

        public void AddUser(UserEntity userEntity, UserStatus userStatus)
        {
            if (userEntity == null)
                throw new ArgumentNullException(nameof(userEntity), "User to be added can't be null");

            if (CustomerUserPermissions.Any(x => x.CustomerId == userEntity.Id))
                throw new BusinessRuleViolationException(userEntity.Id, "User " + userEntity.Id + " already exists for Customer " + Id);

            CustomerUserPermissions.Add(new CustomerUserPermissionEntity() { User = userEntity, UserStatus = userStatus });
        }

        public IEnumerable<UserEntity> GetUsersAbleToProcess(StageEntity stageEntity)
        {
            return FlowSteps
                .Where(xxx=>xxx.From==stageEntity)
                .SelectMany(x => x.FlowStepUserPermissions)
                .Select(xx => xx.User)
                .Distinct()
                .ToArray();
        }
    }
}