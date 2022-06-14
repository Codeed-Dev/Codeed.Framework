using Codeed.Framework.Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sample.Identity
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("USERS");
            builder.Property(u => u.Id).HasColumnName("ID")
                                        .IsRequired();

            builder.Property(u => u.CreatedDate).HasColumnName("CREATE_DATE")
                                                .IsRequired();

            builder.Property(u => u.Uid).HasColumnName("UID")
                                        .IsRequired();

            builder.Property(u => u.Name).HasColumnName("NAME")
                                         .IsRequired();

            builder.Property(u => u.Email).HasColumnName("EMAIL");
            builder.Property(u => u.ImageUrl).HasColumnName("IMAGE_URL");

            builder.HasIndex(u => u.Uid).HasDatabaseName("USER_UID_UK").IsUnique();
        }
    }
}
