using System.Collections.Generic;
using System.Linq;
using Domain.SharedKernel;

namespace Domain.Entities
{
    public class CustomerEntity : BaseEntity
    {
        private CustomerEntity()
        {
            Steps = new List<FlowStepEntity>();
            Users = new List<UserEntity>();
        }

        public CustomerEntity(string name) : this()
        {
            Name = name;
        }

        public ICollection<FlowStepEntity> Steps { get; }
        public ICollection<UserEntity> Users { get; }
        public string Name { get; set; }

        //public IEnumerable<FlowStepEntity> GetNextSteps(StageEntity newTeStage)
        //{
        //    return Steps
        //        .Where(x => x.From == newTeStage)
        //        .ToArray();
        //}
    }
}