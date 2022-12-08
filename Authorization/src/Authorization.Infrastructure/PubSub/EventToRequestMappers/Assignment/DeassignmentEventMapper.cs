using Assignment.SDK.Events;
using Authorization.Application.Features.Assignment;
using Common.SDK;
using Inbox.SDK.EventToRequest;
using MediatR;

namespace Authorization.Infrastructure.PubSub.EventToRequestMappers.Assignment
{
    public class DeassignmentEventMapper : EventToRequestMapper<DeassignmentEvent>
    {
        public override IRequest<Result> Map(DeassignmentEvent @event)
        {
            return new Deassign(@event.Assignment.Id);
        }
    }
}
