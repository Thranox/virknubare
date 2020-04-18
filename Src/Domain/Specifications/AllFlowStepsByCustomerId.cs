using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class AllFlowStepsByCustomerId: ISpecification<FlowStepEntity>
    {
        public AllFlowStepsByCustomerId(Guid customerId)
        {
            Criteria = e => true;
        }

        public Expression<Func<FlowStepEntity, bool>> Criteria { get; }
    }
}