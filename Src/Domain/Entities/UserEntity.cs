﻿using System;
using System.Collections.Generic;
using System.Linq;
using Domain.SharedKernel;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class UserEntity : BaseEntity
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
        public ICollection<FlowStepUserPermissionEntity> FlowStepUserPermissions { get; set; }
        public ICollection<CustomerUserPermissionEntity>  CustomerUserPermissions { get; set; }
        public ICollection<TravelExpenseEntity> TravelExpenses { get; set; }

        public Dictionary<string,string> GetMessageValues()
        {
            return new Dictionary<string, string>();
        }

        public bool IsUserAdminForCustomer(Guid customerId)
        {
            return CustomerUserPermissions
                .Any(x => x.CustomerId == customerId && x.UserStatus == UserStatus.UserAdministrator);

        }
    }
}