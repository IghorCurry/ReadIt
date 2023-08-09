using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadIt.DAL.Entities;

namespace ReadIt.DAL.Persistance.Configuration
{
    internal class UserBookConfiguration : IEntityTypeConfiguration<UserBook>
    {
        public void Configure(EntityTypeBuilder<UserBook> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserBook) //?
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Book)
                .WithMany()
                .HasForeignKey(x => x.BookId);

            builder.HasIndex(b => b.Progress)
            .IsUnique();
        }
    }
}
