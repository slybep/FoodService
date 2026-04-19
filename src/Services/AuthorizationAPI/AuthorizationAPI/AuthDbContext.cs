using AuthorizationAPI.Models;
using Microsoft.EntityFrameworkCore;
using AuthorizationAPI.Models.Enums;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace AuthorizationAPI
{
    public class AuthDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AuthDbContext(
            DbContextOptions<AuthDbContext> options,
            IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(u => u.CreatedAt);

                entity.HasMany(e => e.Roles)
                    .WithMany(e => e.Users)
                    .UsingEntity<UserRole>(
                        right => right.HasOne(ur => ur.Role).WithMany().HasForeignKey(ur => ur.RoleId),
                        left => left.HasOne(ur => ur.User).WithMany().HasForeignKey(ur => ur.UserId));
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasMany(e => e.Permissions)
                    .WithMany(e => e.Roles)
                    .UsingEntity<RolePermission>(
                        l => l.HasOne(ur => ur.Permission).WithMany().HasForeignKey(e => e.PermissionId),
                        r => r.HasOne(ur => ur.Role).WithMany().HasForeignKey(e => e.RoleId));

                var roles = Enum.GetValues<Roles>()
                    .Select(r => new Role { Id = (int)r, Name = r.ToString() });
                entity.HasData(roles);
            });

            modelBuilder.Entity<Permissions>(entity =>
            {
                entity.HasKey(e => e.Id);

                var permissions = Enum.GetValues<Permission>()
                    .Select(p => new Permissions { Id = (int)p, Name = p.ToString() });
                entity.HasData(permissions);
            });

            var authOptions = _configuration
                .GetSection(nameof(AuthorizationOptions))
                .Get<AuthorizationOptions>() ?? new AuthorizationOptions();

            var roleEnumMap = Enum.GetValues<Roles>().ToDictionary(r => r.ToString(), r => (int)r);
            var permEnumMap = Enum.GetValues<Permission>().ToDictionary(p => p.ToString(), p => (int)p);

            var rolePermissionData = new List<RolePermission>();
            foreach (var item in authOptions.RolePermissions) 
            {
                if (!roleEnumMap.TryGetValue(item.Role, out var roleId))
                    continue;

                foreach (var permName in item.Permissions)
                {
                    if (permEnumMap.TryGetValue(permName, out var permId))
                    {
                        rolePermissionData.Add(new RolePermission
                        {
                            RoleId = roleId,
                            PermissionId = permId
                        });
                    }
                }
            }

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.PermissionId });
                entity.HasData(rolePermissionData);
            });
        }
    }
}

