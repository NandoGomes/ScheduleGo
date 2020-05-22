using System.Collections.Generic;
using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities
{
	public class Classroom : Entity
	{
		public string Description { get; private set; }
		public int Capacity { get; private set; }
		public ClassroomType ClassroomType { get; private set; }
		public List<Tag> CategoryTags { get; private set; }
		public List<TimePeriod> AvailablePeriods { get; private set; }

		public bool IsAvailable(TimePeriod timePeriod) => AvailablePeriods.Contains(timePeriod);
	}
}