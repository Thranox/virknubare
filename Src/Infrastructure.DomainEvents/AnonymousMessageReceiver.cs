using System.Collections.Generic;
using Domain.Interfaces;

namespace Infrastructure.DomainEvents
{
    public class AnonymousMessageReceiver : IMessageReceiver
    {
        public AnonymousMessageReceiver(string email)
        {
            Email = email;
        }

        public string Email { get; }
        public void Enrich(Dictionary<string, string> messageValues)
        {
            
        }
    }
}