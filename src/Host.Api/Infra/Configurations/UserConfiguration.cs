using Host.Api.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Host.Api.Infra.Mappers
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Username).IsRequired();

            builder.Property(x => x.Login).IsRequired();

            builder.OwnsOne(x => x.Email, x =>
            {
                x.Property(y => y.Value).IsRequired();
            });

            builder.OwnsOne(x => x.Password, x =>
            {
                x.Property(y => y.Hash).IsRequired();
            });

            builder
                .HasMany(x => x.Projects)
                .WithMany(x => x.Users)
                ;

            builder
                .HasMany(x => x.WorkTimes)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                ;
        }
    }
}
