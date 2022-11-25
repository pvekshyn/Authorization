using Assignment.SDK.DTO;
using Common.SDK;

namespace Assignment.SDK.Events;
public class AssignmentEvent : IEvent
{
    public AssignmentDto Assignment { get; init; }
}
