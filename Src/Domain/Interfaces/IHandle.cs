using System.Threading.Tasks;
using Domain.SharedKernel;

namespace Domain.Interfaces
{
    public interface IHandle<T> where T : BaseDomainEvent
    {
        Task HandleAsync(T travelExpenseChangedStateDomainEvent);
    }
}