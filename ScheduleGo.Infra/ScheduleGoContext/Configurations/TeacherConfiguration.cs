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

			builder.HasMany(teacher => teacher.PreferredCourses)
				.WithOne(link => link.Left)
				.OnDelete(DeleteBehavior.ClientCascade)
				.IsRequired();

			builder.HasMany(teacher => teacher.QualifiedCourses)
				.WithOne(link => link.Left)
				.OnDelete(DeleteBehavior.ClientCascade)
				.IsRequired();

			builder.HasMany(teacher => teacher.PreferredPeriods)
				.WithOne(link => link.Left)
				.OnDelete(DeleteBehavior.ClientCascade)
				.IsRequired();

			builder.HasMany(teacher => teacher.AvailablePeriods)
				.WithOne(link => link.Left)
				.OnDelete(DeleteBehavior.ClientCascade)
				.IsRequired();
		}
	}
}