using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IMessageValueEnricher
    {
        void Enrich(Dictionary<string, string> messageValues);
    }
}