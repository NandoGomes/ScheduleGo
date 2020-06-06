using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleGo.Domain.ScheduleGoContext.Entities;

namespace ScheduleGo.Infra.ScheduleGoContext.Configurations
{
	public class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
	{
		public void Configure(EntityTypeBuilder<Classroom> builder)
		{
			builder.ToTable("Classrooms");

			builder.HasKey(classroom => classroom.Id);

			builder.Property(classroom => classroom.Description).IsRequired();
			builder.Property(classroom => classroom.Capacity).IsRequired();
			builder.Property(classroom => classroom.ClassroomTypeId).IsRequired();

			builder.HasOne(classroom => classroom.ClassroomType)
				.WithMany()
				.OnDelete(DeleteBehavior.NoAction)
				.IsRequired();

			builder.Ignore(classroom => classroom.CategoryTags);
			builder.Ignore(classroom => classroom.AvailablePeriods);
		}
	}
}