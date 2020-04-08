using System;
using Domain.Interfaces;

namespace Infrastructure.Data
{
    public interface IUnitOfWork:IDisposable
    {
        IRepository Repository { get; }
        void Commit();
    }
}