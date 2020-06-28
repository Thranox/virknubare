using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class GetAllLossOfEarningSpec : ISpecification<LossOfEarningSpecEntity>
    {
        public GetAllLossOfEarningSpec()
        {
            Criteria = e => true;
        }
        public Expression<Func<LossOfEarningSpecEntity, bool>> Criteria { get; }
    }
}