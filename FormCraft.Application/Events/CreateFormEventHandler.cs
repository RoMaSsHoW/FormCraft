using FormCraft.Application.Common.Messaging;
using FormCraft.Domain.Events;
using MassTransit;

namespace FormCraft.Application.Events
{
    public class CreateFormEventHandler : IDomainEventHandler<CreateForm>
    {
        public async Task Consume(ConsumeContext<CreateForm> context)
        {

        }
    }
}
