using AuthorizationAPI.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AuthorizationAPI
{
    public class AuthDbContext : DbContext
    {
            private readonly IConfiguration _configuration;

            public AuthDbContext(DbContextOptions<AuthDbContext> options,
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


                });
                modelBuilder.Entity<Role>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasIndex(e => e.Name).IsUnique();

                });
                modelBuilder.Entity<UserRole>(entity =>
                { 
                    entity.HasKey(u => new { u.UserId, u.RoleId });

                    entity.HasOne(u => u.User)
                          .WithMany(u => u.UserRole)
                          .HasForeignKey(ur => ur.UserId)
                          .OnDelete(DeleteBehavior.Cascade);

                    entity.HasOne(u => u.Role)
                          .WithMany(u => u.UserRole)
                          .HasForeignKey(ur => ur.RoleId)
                          .OnDelete(DeleteBehavior.Cascade);
                });

            }
        }
    }

