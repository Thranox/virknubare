using System;
using Domain.SharedKernel;

namespace Domain.Interfaces
{
    public interface IDomainEventDispatcher
    {
        void Dispatch(BaseDomainEvent domainEvent);
        void SetServiceProvider(IServiceProvider serviceProvider);
    }
}