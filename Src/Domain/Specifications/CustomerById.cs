using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class CustomerById : ISpecification<CustomerEntity>
    {
        public CustomerById(Guid customerId)
        {
            Criteria = e => e.Id == customerId;
        }

        public Expression<Func<CustomerEntity, bool>> Criteria { get; }
    }
}