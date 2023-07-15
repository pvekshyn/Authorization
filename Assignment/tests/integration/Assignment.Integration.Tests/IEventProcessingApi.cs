using Refit;
using Role.SDK.Events;

namespace Assignment.SDK.Features
{
    public interface IEventProcessingApi
    {
        [Post("/rolecreated")]
        Task RoleCreated(RoleCreatedEvent roleCreatedEvent);

        [Post("/roledeleted")]
        Task RoleDeleted(RoleDeletedEvent roleDeletedEvent);
    }
}
