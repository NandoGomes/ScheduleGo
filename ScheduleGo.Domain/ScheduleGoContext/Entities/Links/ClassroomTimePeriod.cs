using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities.Links
{
	public class ClassroomTimePeriod : LinkEntity<Classroom, TimePeriod>
	{
		protected ClassroomTimePeriod() { }
		public ClassroomTimePeriod(Classroom left, TimePeriod right) : base(left, right) { }
	}
}