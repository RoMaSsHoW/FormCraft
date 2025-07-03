using MediatR;

namespace FormCraft.Application.Common.Messaging
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand>
        where TCommand : ICommand
    {
    }
}
