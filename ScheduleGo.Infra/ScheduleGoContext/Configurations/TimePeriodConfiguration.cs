using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleGo.Domain.ScheduleGoContext.Entities;

namespace ScheduleGo.Infra.ScheduleGoContext.Configurations
{
	public class TimePeriodConfiguration : IEntityTypeConfiguration<TimePeriod>
	{
		public void Configure(EntityTypeBuilder<TimePeriod> builder)
		{
			builder.ToTable("TimePeriods");

			builder.HasKey(timePeriod => timePeriod.Id);

			builder.Property(timePeriod => timePeriod.Description).IsRequired();
			builder.Property(timePeriod => timePeriod.Start).IsRequired();
			builder.Property(timePeriod => timePeriod.End).IsRequired();
		}
	}
}