using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities.Links
{
	public class TeacherQualifiedCourse : LinkEntity<Teacher, Course>
	{
		protected TeacherQualifiedCourse() { }
		public TeacherQualifiedCourse(Teacher left, Course right) : base(left, right) { }
	}
}