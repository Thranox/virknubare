using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class TravelExpenseByUserId : ISpecification<TravelExpenseEntity>
    {
        public TravelExpenseByUserId(Guid id)
        {
            Criteria = e => e.OwnedByUser.Id == id;
        }

        public Expression<Func<TravelExpenseEntity, bool>> Criteria { get; }
    }
}