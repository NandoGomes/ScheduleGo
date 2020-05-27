using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities.Links
{
	public class CourseTag : LinkEntity<Course, Tag>
	{
		protected CourseTag() { }
		public CourseTag(Course left, Tag right) : base(left, right) { }
	}
}