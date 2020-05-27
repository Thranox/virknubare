using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class UserBySub : ISpecification<UserEntity>
    {
        public UserBySub(string sub)
        {
            Criteria = e => e.Subject == sub;
        }

        public Expression<Func<UserEntity, bool>> Criteria { get; }
    }
}