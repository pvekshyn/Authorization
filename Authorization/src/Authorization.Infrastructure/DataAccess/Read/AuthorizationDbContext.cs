using Authorization.Domain;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Infrastructure.DataAccess.Read;

internal class AuthorizationDbContext : DbContext, IAuthorizationDbContext
{

    public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options) { }

    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Domain.Role> Roles { get; set; }
    public DbSet<AssignmentViewEntry> AssignmentViewEntries { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Domain.Role>(entity =>
        {
            entity.ToTable("Role");
            entity.HasKey(x => x.Id);
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
        });

        builder.Entity<AssignmentViewEntry>(entity =>
        {
            entity.ToTable("AssignmentViewEntry");
            entity.HasNoKey();
        });
    }
}