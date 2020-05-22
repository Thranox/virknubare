using Domain.ValueObjects;

namespace Domain.Interfaces
{
    public interface IMessageTemplateService
    {
        IMessageTemplate Get(MessageKind messageKind);
    }
}