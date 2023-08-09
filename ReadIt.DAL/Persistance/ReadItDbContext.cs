using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReadIt.DAL.Entities;
using ReadIt.DAL.Persistance.Configuration;
using ReadIt.DAL.Persistance.Settings;

namespace ReadIt.DAL.Persistance
{
    public class ReadItDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        private DefaultAdminSettings _defaultAdminSettings { get; init; }
        public ReadItDbContext(DbContextOptions<ReadItDbContext> options, IOptions<DefaultAdminSettings> defaultAdminSettings)
                : base(options) => _defaultAdminSettings = defaultAdminSettings.Value;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserBookConfiguration());
            modelBuilder.ApplyConfiguration(new ReadSessionConfiguration());
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<ReadSession> ReadSessions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }
    }
}
