using System;
using System.Collections.Generic;
using Domain.SharedKernel;

namespace Domain.Interfaces
{
    public interface IRepository
    {
        T GetById<T>(Guid id) where T : BaseEntity;
        List<T> List<T>(ISpecification<T> spec = null) where T : BaseEntity;
        T Add<T>(T entity) where T : BaseEntity;
        void Update<T>(T entity) where T : BaseEntity;
        void Delete<T>(T entity) where T : BaseEntity;
        void Commit();
    }
}