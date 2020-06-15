using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class WaitingEmails : ISpecification<EmailEntity>
    {
        public WaitingEmails()
        {
            Criteria = e => e.SendTime == null;
        }

        public Expression<Func<EmailEntity, bool>> Criteria { get; }
    }
}