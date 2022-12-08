using Authorization.Infrastructure.DataAccess.Read.Models;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Infrastructure.DataAccess.Read;

public interface IAuthorizationDbContext
{
    DbSet<Permission> Permissions { get; set; }
    DbSet<Models.Role> Roles { get; set; }
    DbSet<AssignmentViewEntry> AssignmentViewEntries { get; set; }
}

internal class AuthorizationDbContext : DbContext, IAuthorizationDbContext
{

    public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options) { }

    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Models.Role> Roles { get; set; }
    public DbSet<AssignmentViewEntry> AssignmentViewEntries { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Models.Role>(entity =>
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
                    .HasOne<Models.Role>()
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