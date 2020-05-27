using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Specifications
{
    public class StageByValue : ISpecification<StageEntity>
    {
        public StageByValue(TravelExpenseStage value)
        {
            Criteria = e => e.Value == value;
        }

        public Expression<Func<StageEntity, bool>> Criteria { get; }
    }
}