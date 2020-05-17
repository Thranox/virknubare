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
            Key = key;
            Description = description;
        }

        private FlowStepEntity(StageEntity @from, CustomerEntity customer)
        {
            From = from;
            Customer = customer;
        }

        public StageEntity From { get; set; }
        public CustomerEntity Customer { get; set; }

        public string Key { get; set; }

        public string Description { get; set; }

        //public TravelExpenseStage From { get; set; }
        public ICollection<FlowStepUserPermissionEntity> FlowStepUserPermissions { get; set; }
    }
}