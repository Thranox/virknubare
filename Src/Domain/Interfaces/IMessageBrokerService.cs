using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Interfaces
{
    public interface IMessageBrokerService 
    {
        Task SendMessageAsync(IEnumerable<UserEntity> userEntities, TravelExpenseEntity travelExpenseEntity, MessageKind messageKind);
    }
}