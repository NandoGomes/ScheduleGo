using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities.Links
{
	public class ClassroomTag : LinkEntity<Classroom, Tag>
	{
		protected ClassroomTag() { }
		public ClassroomTag(Classroom left, Tag right) : base(left, right) { }
	}
}