﻿using System.Collections.Generic;
using Domain.SharedKernel;

namespace Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        private UserEntity()
        {
            FlowStepUserPermissions=new List<FlowStepUserPermissionEntity>();
            CustomerUserPermissions=new List<CustomerUserPermissionEntity>();
        }

        public UserEntity(string name, string subject) : this()
        {
            Name = name;
            Subject = subject;
        }

        public string Name { get; set; }
        public string Subject { get; set; }
        public ICollection<FlowStepUserPermissionEntity> FlowStepUserPermissions { get; set; }
        public ICollection<CustomerUserPermissionEntity>  CustomerUserPermissions { get; set; }
    }
}