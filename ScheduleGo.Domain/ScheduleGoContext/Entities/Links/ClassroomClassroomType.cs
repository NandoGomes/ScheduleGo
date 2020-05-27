using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities.Links
{
	public class ClassroomClassroomType : LinkEntity<Classroom, ClassroomType>
	{
		protected ClassroomClassroomType() { }
		public ClassroomClassroomType(Classroom left, ClassroomType right) : base(left, right) { }
	}
}