using System.Collections.Generic;
using Domain.SharedKernel;

namespace Domain.Entities
{
    public class FlowStepEntity:BaseEntity
    {
        private FlowStepEntity()
        {
            FlowStepUserPermissions=new List<FlowStepUserPermissionEntity>();
        }

        public FlowStepEntity(string key, TravelExpenseStage @from):this()
        {
            Key = key;
            From = @from;
        }
        public string Key { get; set; }
        public TravelExpenseStage From { get; set; }
        public ICollection<FlowStepUserPermissionEntity> FlowStepUserPermissions { get; set; }
    }
}