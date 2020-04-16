using System.Collections.Generic;
using System.Linq;
using Domain.SharedKernel;

namespace Domain.Entities
{
    public class CustomerEntity : BaseEntity
    {
        private CustomerEntity()
        {
            FlowSteps = new List<FlowStepEntity>();
            Users = new List<UserEntity>();
            TravelExpenses = new List<TravelExpenseEntity>();
        }

        public CustomerEntity(string name) : this()
        {
            Name = name;
        }

        public ICollection<TravelExpenseEntity> TravelExpenses { get; }
        public ICollection<FlowStepEntity> FlowSteps { get; }
        public ICollection<UserEntity> Users { get; }
        public string Name { get; set; }
    }
}