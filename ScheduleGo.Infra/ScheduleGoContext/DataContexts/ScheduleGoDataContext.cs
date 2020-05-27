using Microsoft.EntityFrameworkCore;
using ScheduleGo.Domain.ScheduleGoContext.Entities;
using ScheduleGo.Domain.ScheduleGoContext.Entities.Links;
using ScheduleGo.Infra.ScheduleGoContext.Configurations;
using ScheduleGo.Infra.ScheduleGoContext.Configurations.Links;

namespace ScheduleGo.Infra.ScheduleGoContext.DataContexts
{
	public class ScheduleGoDataContext : DbContext
	{
		public ScheduleGoDataContext() { }
		public ScheduleGoDataContext(DbContextOptions<ScheduleGoDataContext> options) : base(options) { }

		public DbSet<Classroom> Classrooms { get; set; }
		public DbSet<ClassroomType> ClassroomTypes { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<Teacher> Teachers { get; set; }
		public DbSet<TimePeriod> TimePeriods { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new ClassroomConfiguration());
			modelBuilder.ApplyConfiguration(new ClassroomTypeConfiguration());
			modelBuilder.ApplyConfiguration(new CourseConfiguration());
			modelBuilder.ApplyConfiguration(new TagConfiguration());
			modelBuilder.ApplyConfiguration(new TeacherConfiguration());
			modelBuilder.ApplyConfiguration(new TimePeriodConfiguration());

			modelBuilder.ApplyConfiguration(new LinkEntityConfiguration<ClassroomClassroomType, Classroom, ClassroomType>());
			modelBuilder.ApplyConfiguration(new LinkEntityConfiguration<ClassroomTag, Classroom, Tag>());
			modelBuilder.ApplyConfiguration(new LinkEntityConfiguration<ClassroomTimePeriod, Classroom, TimePeriod>());
			modelBuilder.ApplyConfiguration(new LinkEntityConfiguration<CourseClassroomType, Course, ClassroomType>());
			modelBuilder.ApplyConfiguration(new LinkEntityConfiguration<CourseTag, Course, Tag>());
			modelBuilder.ApplyConfiguration(new LinkEntityConfiguration<CourseTimePeriod, Course, TimePeriod>());
			modelBuilder.ApplyConfiguration(new LinkEntityConfiguration<TeacherAvailablePeriod, Teacher, TimePeriod>());
			modelBuilder.ApplyConfiguration(new LinkEntityConfiguration<TeacherPreferredCourse, Teacher, Course>());
			modelBuilder.ApplyConfiguration(new LinkEntityConfiguration<TeacherPreferredPeriod, Teacher, TimePeriod>());
			modelBuilder.ApplyConfiguration(new LinkEntityConfiguration<TeacherQualifiedCourse, Teacher, Course>());
		}
	}
}