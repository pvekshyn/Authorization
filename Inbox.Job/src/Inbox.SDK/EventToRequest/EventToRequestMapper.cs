using Common.SDK;
using MediatR;

namespace Inbox.SDK.EventToRequest
{
    public abstract class EventToRequestMapper<TEvent> :
    IEventToRequestMapper,
    IEventToRequestMapper<TEvent> where TEvent : IEvent
    {
        public IRequest<Result> Map(IEvent @event)
            => Map((TEvent)@event);

        public abstract IRequest<Result> Map(TEvent @event);
    }


}
