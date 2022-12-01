using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Role.Domain;
using Role.Domain.ValueObjects.Role;
using Role.Domain.ValueObjects.Permission;
using Role.Application.Dependencies;
using Outbox.SDK.Models;

namespace Role.Infrastructure;

public class RoleDbContext : DbContext, IRoleDbContext
{
    public RoleDbContext(DbContextOptions<RoleDbContext> options) : base(options) { }

    public DbSet<Domain.Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Domain.Role>(entity =>
        {
            entity.ToTable("Role");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id)
                .HasConversion(
                    v => v.Value,
                    v => new RoleId(v));
            entity.Property(x => x.Name)
                .HasConversion(
                    v => v.Value,
                    v => new RoleName(v));

            entity.HasMany(x => x.Permissions)
            .WithMany(x => x.Roles)
            .UsingEntity<Dictionary<string, object>>(
                "RolePermission",
                j => j
                    .HasOne<Permission>()
                    .WithMany()
                    .HasForeignKey("PermissionId"),
                j => j
                    .HasOne<Domain.Role>()
                    .WithMany()
                    .HasForeignKey("RoleId"));
        });

        builder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permission");
            entity.HasKey(x => x.Id);
            entity.HasMany(p => p.Roles)
            .WithMany(b => b.Permissions);
        });

        builder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("OutboxMessage");
            entity.HasKey(x => x.Id);
        });
    }

    public async Task AddPubSubOutboxMessageAsync(Guid entityId, object pubSubEvent, CancellationToken cancellationToken)
    {
        var pubSubOutboxMessage = OutboxMessage.CreatePubSubMessage(entityId, pubSubEvent);
        await AddAsync(pubSubOutboxMessage, cancellationToken);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<PermissionId>()
            .HaveConversion<PermissionIdConverter>();
        configurationBuilder
            .Properties<PermissionName>()
            .HaveConversion<PermissionNameConverter>();
    }

    public class PermissionIdConverter : ValueConverter<PermissionId, Guid>
    {
        public PermissionIdConverter() : base(
                v => v.Value,
                v => new PermissionId(v))
        { }
    }

    public class PermissionNameConverter : ValueConverter<PermissionName, string>
    {
        public PermissionNameConverter() : base(
                v => v.Value,
                v => new PermissionName(v))
        { }
    }
}
