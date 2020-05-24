using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.ValueObjects;

namespace Domain.Interfaces
{
    public interface IMessageBrokerService 
    {
        Task<int> SendMessageAsync(IEnumerable<IMessageReceiver> messageReceivers,
            MessageKind messageKind, IMessageValueEnricher[] messageValueEnrichers);
    }
}