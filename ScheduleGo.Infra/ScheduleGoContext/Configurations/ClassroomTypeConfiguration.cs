using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleGo.Domain.ScheduleGoContext.Entities;

namespace ScheduleGo.Infra.ScheduleGoContext.Configurations
{
	public class ClassroomTypeConfiguration : IEntityTypeConfiguration<ClassroomType>
	{
		public void Configure(EntityTypeBuilder<ClassroomType> builder)
		{
			builder.ToTable("ClassroomTypes");

			builder.HasKey(classroomType => classroomType.Id);

			builder.Property(classroomType => classroomType.Description).IsRequired();
		}
	}
}