using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class UserBySubSpecification : ISpecification<UserEntity>
    {
        public UserBySubSpecification(string sub)
        {
            Criteria = e => e.Subject == sub;

        }

        public Expression<Func<UserEntity, bool>> Criteria { get; }
    }
}