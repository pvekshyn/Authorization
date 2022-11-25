using Common.SDK;
using MediatR;

namespace Inbox.SDK.EventToRequest
{
    public interface IEventToRequestMapper
    {
        IRequest<Result> Map(IEvent @event);
    }

    internal interface IEventToRequestMapper<T> where T : IEvent
    {
        IRequest<Result> Map(T @event);
    }
}