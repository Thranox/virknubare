using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class TravelExpenseByCustomerList : ISpecification<TravelExpenseEntity>
    {
        public TravelExpenseByCustomerList(IEnumerable<Guid> customersVisibleByUser)
        {
            Criteria = e => customersVisibleByUser.Contains(e.Customer.Id);
        }

        public Expression<Func<TravelExpenseEntity, bool>> Criteria { get; }
    }
}