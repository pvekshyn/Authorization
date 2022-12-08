using Authorization.Application.Features.Permission;
using Common.SDK;
using Inbox.SDK.EventToRequest;
using MediatR;
using Role.SDK.Events;

namespace Authorization.Infrastructure.PubSub.EventToRequestMappers.Permission
{
    public class PermissionDeletedEventMapper : EventToRequestMapper<PermissionDeletedEvent>
    {
        public override IRequest<Result> Map(PermissionDeletedEvent @event)
        {
            return new DeletePermission(@event.Id);
        }
    }
}
