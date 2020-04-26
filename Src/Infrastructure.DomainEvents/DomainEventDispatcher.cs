using System;
using System.Linq;
using Domain.Interfaces;
using Domain.SharedKernel;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DomainEvents
{
    // https://gist.github.com/jbogard/54d6569e883f63afebc7
    // http://lostechies.com/jimmybogard/2014/05/13/a-better-domain-events-pattern/
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Dispatch(BaseDomainEvent domainEvent)
        {
            var handlerType = typeof(IHandle<>).MakeGenericType(domainEvent.GetType());
            var wrapperType = typeof(DomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            using (var serviceScope = _serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var handlers = serviceScope.ServiceProvider.GetServices(handlerType);
                var wrappedHandlers = handlers
                    .Cast<object>()
                    .Select(handler => (DomainEventHandler) Activator.CreateInstance(wrapperType, handler));

                foreach (var handler in wrappedHandlers)
                {
                    handler.Handle(domainEvent);
                }
            }
        }

        private abstract class DomainEventHandler
        {
            public abstract void Handle(BaseDomainEvent domainEvent);
        }

        private class DomainEventHandler<T> : DomainEventHandler
            where T : BaseDomainEvent
        {
            private readonly IHandle<T> _handler;

            public DomainEventHandler(IHandle<T> handler)
            {
                _handler = handler;
            }

            public override void Handle(BaseDomainEvent domainEvent)
            {
                _handler.Handle((T)domainEvent);
            }
        }
    }
}