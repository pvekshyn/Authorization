using Assignment.SDK.DTO;
using Common.SDK;

namespace Assignment.SDK.Events;
public class DeassignmentEvent : IEvent
{
    public AssignmentDto Assignment { get; init; }
}
