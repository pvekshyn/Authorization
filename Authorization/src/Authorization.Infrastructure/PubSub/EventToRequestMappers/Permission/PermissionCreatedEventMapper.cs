using Authorization.Application.Features.Permission;
using Common.SDK;
using Inbox.SDK.EventToRequest;
using MediatR;
using Role.SDK.Events;

namespace Authorization.Infrastructure.PubSub.EventToRequestMappers.Permission
{
    public class PermissionCreatedEventMapper : EventToRequestMapper<PermissionCreatedEvent>
    {
        public override IRequest<Result> Map(PermissionCreatedEvent @event)
        {
            return new CreatePermission(@event.Permission);
        }
    }
}
