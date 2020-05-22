using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Specifications
{
    public class FlowStepByCustomer : ISpecification<FlowStepEntity>
    {
        public FlowStepByCustomer(TravelExpenseStage travelExpenseStage, Guid customerId)
        {
            Criteria = e => e.From.Value == travelExpenseStage && e.Customer.Id == customerId;
        }

        public Expression<Func<FlowStepEntity, bool>> Criteria { get; }
    }
}