using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleGo.Domain.ScheduleGoContext.Entities;

namespace ScheduleGo.Infra.ScheduleGoContext.Configurations
{
	public class TagConfiguration : IEntityTypeConfiguration<Tag>
	{
		public void Configure(EntityTypeBuilder<Tag> builder)
		{
			builder.ToTable("Tags");

			builder.HasKey(tag => tag.Id);

			builder.Property(tag => tag.Description).IsRequired();
		}
	}
}