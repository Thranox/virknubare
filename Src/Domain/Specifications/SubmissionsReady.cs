using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class SubmissionsReady : ISpecification<SubmissionEntity>
    {
        public SubmissionsReady()
        {
            Criteria = e => e.SubmissionTime == null;
        }

        public Expression<Func<SubmissionEntity, bool>> Criteria { get; }
    }
}