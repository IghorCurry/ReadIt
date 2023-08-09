using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadIt.DAL.Entities;

namespace ReadIt.DAL.Persistance.Configuration
{
    internal class ReadSessionConfiguration : IEntityTypeConfiguration<ReadSession>
    {
        public void Configure(EntityTypeBuilder<ReadSession> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(rs => rs.User)
                .WithMany(u => u.ReadSessions)
                .HasForeignKey(rs => rs.UserId);
        }
    }
}
