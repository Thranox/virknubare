using System;
using System.Linq;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class LossOfEarningSpecByAmountAndCustomer : ISpecification<LossOfEarningSpecEntity>
    {
        public LossOfEarningSpecByAmountAndCustomer(int rate, Guid customerId)
        {
            Criteria = e => e.Rate == rate && e.CustomerId==customerId;
        }

        public Expression<Func<LossOfEarningSpecEntity, bool>> Criteria { get; }
    }
    public class AllFlowStepsByUserId : ISpecification<FlowStepEntity>
    {
        public AllFlowStepsByUserId(Guid userId)
        {
            Criteria = e => e.FlowStepUserPermissions.Any(x => x.UserId == userId);
        }

        public Expression<Func<FlowStepEntity, bool>> Criteria { get; }
    }
}