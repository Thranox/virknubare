using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Specifications
{
    public class FlowStepByCustomerAndStage:ISpecification<FlowStepEntity>
    {
        public FlowStepByCustomerAndStage(Guid customerId, TravelExpenseStage stageValue)
        {
            Criteria = e => e.Customer.Id == customerId && e.From.Value==stageValue;
        }

        public Expression<Func<FlowStepEntity, bool>> Criteria { get; }
    }
}