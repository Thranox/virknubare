using System;
using Domain.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IRepository repository)
        {
            Repository = repository;
        }

        public void Dispose()
        {
        }

        public IRepository Repository { get; }

        public void Commit()
        {
            Repository.Commit();
        }
    }
}