using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class CustomerByName : ISpecification<CustomerEntity>
    {
        public CustomerByName(string name)
        {
            Criteria = e => e.Name == name;
        }

        public Expression<Func<CustomerEntity, bool>> Criteria { get; }
    }
}