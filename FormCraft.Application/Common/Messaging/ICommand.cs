using MediatR;

namespace FormCraft.Application.Common.Messaging
{
    public interface ICommand : IRequest
    {
    }

    public interface ICommand<TResponse> : IRequest<TResponse>
    {
    }
}
