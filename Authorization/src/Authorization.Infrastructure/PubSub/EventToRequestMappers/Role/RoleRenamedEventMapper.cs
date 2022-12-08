using Authorization.Application.Features.Role;
using Common.SDK;
using Inbox.SDK.EventToRequest;
using MediatR;
using Role.SDK.Events;

namespace Authorization.Infrastructure.PubSub.EventToRequestMappers.Role
{
    public class RoleRenamedEventMapper : EventToRequestMapper<RoleRenamedEvent>
    {
        public override IRequest<Result> Map(RoleRenamedEvent @event)
        {
            return new RenameRole(@event.Role);
        }
    }
}
