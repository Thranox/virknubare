using System;
using Domain.SharedKernel;

namespace Domain
{
    public class TravelExpenseEntity: BaseEntity
    {
        private TravelExpenseEntity()
        {
            
        }

        public TravelExpenseEntity(string description):this()
        {
            Description = description;
            PublicId = Guid.NewGuid();
        }
        public string Description { get; private set; }
        public Guid PublicId { get; private set; }

        public void Update(string description)
        {
            Description = description;
        }
    }
}
