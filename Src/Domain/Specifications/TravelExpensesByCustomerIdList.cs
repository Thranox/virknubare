using System;
using System.Linq;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class TravelExpensesByCustomerIdList : ISpecification<TravelExpenseEntity>
    {
        public TravelExpensesByCustomerIdList(Guid[] customerIds)
        {
            Criteria = e => customerIds.Contains(e.Customer.Id);
        }
        public Expression<Func<TravelExpenseEntity, bool>> Criteria { get; }
    }
}