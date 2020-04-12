using System;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository Repository { get; }
        void Commit();
    }
}