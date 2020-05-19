using System;
using System.Collections.Generic;
using Domain.SharedKernel;

namespace Domain.Entities
{
    public class FlowStepEntity : BaseEntity
    {
        public FlowStepEntity()
        {
            FlowStepUserPermissions = new List<FlowStepUserPermissionEntity>();
        }

        public FlowStepEntity(string key, StageEntity from, CustomerEntity customer,string description) : this(from,customer)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        private FlowStepEntity(StageEntity @from, CustomerEntity customer):this()
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        }

        public StageEntity From { get; set; }
        public CustomerEntity Customer { get; set; }

        public string Key { get; set; }

        public string Description { get; set; }

        //public TravelExpenseStage From { get; set; }
        public ICollection<FlowStepUserPermissionEntity> FlowStepUserPermissions { get; set; }
    }
}