using MediatR;

namespace FormCraft.Application.Common.Messaging
{
    public interface IQuery<TResult> : IRequest<TResult>
    {
    }
}
