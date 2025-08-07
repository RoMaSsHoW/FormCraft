using FormCraft.Domain.Common;
using MassTransit;

namespace FormCraft.Application.Common.Messaging
{
    public interface IDomainEventHandler<in TEvent> : IConsumer<TEvent>
        where TEvent : class, IDomainEvent
    {
    }
}
