using System;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
    }
}