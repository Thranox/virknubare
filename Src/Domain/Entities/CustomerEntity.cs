using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class CustomerEntity
    {
        public CustomerEntity()
        {
            Steps = new List<FlowStepEntity>();
        }

        public ICollection<FlowStepEntity> Steps { get; }
        public ICollection<UserEntity> Users { get; }

        public IEnumerable<FlowStepEntity> GetNextSteps(StageEntity newTeStage)
        {
            return Steps
                .Where(x => x.From == newTeStage)
                .ToArray();
        }
    }
}