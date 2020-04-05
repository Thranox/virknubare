using System;
using System.Linq.Expressions;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class TravelExpenseByPublicId : ISpecification<TravelExpenseEntity>
    {
        public TravelExpenseByPublicId(Guid publicId)
        {
            Criteria = e =>
                e.PublicId ==publicId;
        }

        public Expression<Func<TravelExpenseEntity, bool>> Criteria { get; }
    }
}