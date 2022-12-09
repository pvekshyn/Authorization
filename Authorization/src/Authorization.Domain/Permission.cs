namespace Authorization.Domain
{
    public class Permission
    {
        public Guid Id { get; init; }
        public string Name { get; init; }

        [Obsolete]//just for EF many to many
        public ICollection<Role> Roles { get; init; }
    }
}