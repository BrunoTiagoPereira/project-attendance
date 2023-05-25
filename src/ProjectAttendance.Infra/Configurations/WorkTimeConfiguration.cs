using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectAttendance.Domain.Projects.Entities;

namespace ProjectAttendance.Infra.Configurations
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