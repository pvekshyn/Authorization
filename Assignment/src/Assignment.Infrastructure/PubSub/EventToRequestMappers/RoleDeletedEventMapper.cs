using Assignment.Application.Features.Role;
using Common.SDK;
using Inbox.SDK.EventToRequest;
using MediatR;
using Role.SDK.Events;

namespace Assignment.Infrastructure.PubSub.EventToRequestMappers
{
    public class RoleDeletedEventMapper : EventToRequestMapper<RoleDeletedEvent>
    {
        public override IRequest<Result> Map(RoleDeletedEvent @event)
        {
            return new DeleteRole(@event.Id);
        }
    }
}
