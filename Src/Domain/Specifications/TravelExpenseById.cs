using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class TravelExpenseById : ISpecification<TravelExpenseEntity>
    {
        public TravelExpenseById(Guid id)
        {
            Criteria = e =>e.Id ==id;
        }

        public Expression<Func<TravelExpenseEntity, bool>> Criteria { get; }
    }
}