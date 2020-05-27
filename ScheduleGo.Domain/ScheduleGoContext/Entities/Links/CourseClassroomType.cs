using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities.Links
{
	public class CourseClassroomType : LinkEntity<Course, ClassroomType>
	{
		protected CourseClassroomType() { }
		public CourseClassroomType(Course left, ClassroomType right) : base(left, right) { }
	}
}