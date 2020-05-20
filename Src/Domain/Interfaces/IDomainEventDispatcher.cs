using System;
using System.Threading.Tasks;
using Domain.SharedKernel;

namespace Domain.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(BaseDomainEvent domainEvent);
    }
}