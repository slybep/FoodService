using Microsoft.EntityFrameworkCore;
using System.Data;
using UserAPI.Models;

namespace UserAPI
{
    public class UserDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public UserDbContext(
            DbContextOptions<UserDbContext> options,
            IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Phone);
            });
        }
    }
}
