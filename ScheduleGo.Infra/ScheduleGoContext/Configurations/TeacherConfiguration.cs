using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleGo.Domain.ScheduleGoContext.Entities;

namespace ScheduleGo.Infra.ScheduleGoContext.Configurations
{
	public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
	{
		public void Configure(EntityTypeBuilder<Teacher> builder)
		{
			builder.ToTable("Teachers");

			builder.HasKey(teacher => teacher.Id);

			builder.Property(teacher => teacher.Name).IsRequired();

			builder.Ignore(teacher => teacher.PreferredCourses);
			builder.Ignore(teacher => teacher.QualifiedCourses);
			builder.Ignore(teacher => teacher.PreferredPeriods);
			builder.Ignore(teacher => teacher.AvailablePeriods);
		}
	}
}