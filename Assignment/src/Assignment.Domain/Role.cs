using Assignment.Domain.ValueObjects;

namespace Assignment.Domain
{
    public class Role
    {
        public RoleId Id { get; init; }

        // just for EF
        private Role()
        {
        }

        public Role(RoleId id)
        {
            Id = id;
        }
    }
}
