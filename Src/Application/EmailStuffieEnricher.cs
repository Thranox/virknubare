using System.Collections.Generic;
using Domain;
using Domain.Interfaces;

namespace Application
{
    public class EmailStuffieEnricher:IMessageValueEnricher
    {
        private readonly string _appUrl;

        public EmailStuffieEnricher(string appUrl)
        {
            _appUrl = appUrl;
        }

        public void Enrich(Dictionary<string, string> messageValues)
        {
            messageValues.Add(KeyMessagesConst.InvitationAppUrl, _appUrl);
        }
    }
}