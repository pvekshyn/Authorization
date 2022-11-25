using Assignment.Application.Features.Role;
using Common.SDK;
using Inbox.SDK.EventToRequest;
using MediatR;
using Role.SDK.Events;

namespace Assignment.Infrastructure.PubSub.EventToRequestMappers
{
    public class RoleCreatedEventMapper : EventToRequestMapper<RoleCreatedEvent>
    {
        public override IRequest<Result> Map(RoleCreatedEvent @event)
        {
            return new CreateRole(@event.Role.Id);
        }
    }
}
