using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities.Links
{
	public class TeacherPreferredPeriod : LinkEntity<Teacher, TimePeriod>
	{
		protected TeacherPreferredPeriod() { }
		public TeacherPreferredPeriod(Teacher left, TimePeriod right) : base(left, right) { }
	}
}