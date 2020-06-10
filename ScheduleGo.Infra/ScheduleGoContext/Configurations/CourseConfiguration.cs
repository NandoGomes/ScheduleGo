using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleGo.Domain.ScheduleGoContext.Entities;

namespace ScheduleGo.Infra.ScheduleGoContext.Configurations
{
	public class CourseConfiguration : IEntityTypeConfiguration<Course>
	{
		public void Configure(EntityTypeBuilder<Course> builder)
		{
			builder.ToTable("Courses");

			builder.HasKey(course => course.Id);

			builder.Property(course => course.Name).IsRequired();
			builder.Property(course => course.Description).IsRequired();
			builder.Property(course => course.StudentsCount).IsRequired();
			builder.Property(course => course.NeededClassroomTypeId).IsRequired();
			builder.Property(course => course.WeeklyWorkload).IsRequired();

			builder.HasOne(course => course.NeededClassroomType)
				.WithMany()
				.OnDelete(DeleteBehavior.NoAction)
				.IsRequired();

			builder.HasMany(course => course.CategoryTags)
				.WithOne(link => link.Left);

			builder.HasMany(course => course.AvailablePeriods)
				.WithOne(link => link.Left);
		}
	}
}