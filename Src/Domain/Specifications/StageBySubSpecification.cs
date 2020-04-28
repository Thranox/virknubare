using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class StageBySubSpecification : ISpecification<StageEntity>
    {
        public StageBySubSpecification(int value)
        {
            Criteria = e => e.Value == value;

        }

        public Expression<Func<StageEntity, bool>> Criteria { get; }
    }
}