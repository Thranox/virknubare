using System;
using System.Threading.Tasks;
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

        public async Task CommitAsync()
        {
            await Repository.CommitAsync();
        }
    }
}