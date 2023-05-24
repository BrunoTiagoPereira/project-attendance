using Host.Api.Domain.Projects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Host.Api.Infra.Mappers
{
    public class WorkTimeConfiguration : IEntityTypeConfiguration<WorkTime>
    {
        public void Configure(EntityTypeBuilder<WorkTime> builder)
        {
            builder.ToTable("WorkTimes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.StartedAt).IsRequired();

            builder.Property(x => x.EndedAt).IsRequired();
        }
    }
}