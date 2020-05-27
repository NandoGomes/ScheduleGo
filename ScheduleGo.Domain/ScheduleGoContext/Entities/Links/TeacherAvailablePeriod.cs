using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities.Links
{
	public class TeacherAvailablePeriod : LinkEntity<Teacher, TimePeriod>
	{
		protected TeacherAvailablePeriod() { }
		public TeacherAvailablePeriod(Teacher left, TimePeriod right) : base(left, right) { }
	}
}