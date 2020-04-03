using CleanArchitecture.Core.SharedKernel;

namespace Domain.Interfaces
{
    public interface IHandle<T> where T : BaseDomainEvent
    {
        void Handle(T domainEvent);
    }
}