using Assignment.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Assignment.Infrastructure;

public class AssignmentDbContext : DbContext
{

    public AssignmentDbContext(DbContextOptions<AssignmentDbContext> options) : base(options) { }

    public DbSet<Domain.Assignment> Assignments { get; set; }
    public DbSet<Domain.Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Domain.Role>(entity =>
        {
            entity.ToTable("Role");
            entity.HasKey(x => x.Id);
        });

        builder.Entity<Domain.Assignment>(entity =>
        {
            entity.ToTable("Assignment");
            entity.HasKey(x => x.Id);
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<AssignmentId>()
            .HaveConversion<AssignmentIdConverter>();
        configurationBuilder
            .Properties<UserId>()
            .HaveConversion<UserIdConverter>();
        configurationBuilder
            .Properties<RoleId>()
            .HaveConversion<RoleIdConverter>();
    }

    public class AssignmentIdConverter : ValueConverter<AssignmentId, Guid>
    {
        public AssignmentIdConverter() : base(
                v => v.Value,
                v => new AssignmentId(v))
        { }
    }

    public class UserIdConverter : ValueConverter<UserId, Guid>
    {
        public UserIdConverter() : base(
                v => v.Value,
                v => new UserId(v))
        { }
    }

    public class RoleIdConverter : ValueConverter<RoleId, Guid>
    {
        public RoleIdConverter() : base(
                v => v.Value,
                v => new RoleId(v))
        { }
    }
}
