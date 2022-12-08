using Assignment.SDK.Events;
using Authorization.Application.Features.Assignment;
using Common.SDK;
using Inbox.SDK.EventToRequest;
using MediatR;

namespace Authorization.Infrastructure.PubSub.EventToRequestMappers.Assignment
{
    public class AssignmentEventMapper : EventToRequestMapper<AssignmentEvent>
    {
        public override IRequest<Result> Map(AssignmentEvent @event)
        {
            return new Assign(@event.Assignment);
        }
    }
}
