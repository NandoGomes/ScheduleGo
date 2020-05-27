using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities.Links
{
	public class TeacherPreferredCourse : LinkEntity<Teacher, Course>
	{
		protected TeacherPreferredCourse() { }
		public TeacherPreferredCourse(Teacher left, Course right) : base(left, right) { }
	}
}