using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities.Links
{
	public class CourseTimePeriod : LinkEntity<Course, TimePeriod>
	{
		protected CourseTimePeriod() { }
		public CourseTimePeriod(Course left, TimePeriod right) : base(left, right) { }
	}
}