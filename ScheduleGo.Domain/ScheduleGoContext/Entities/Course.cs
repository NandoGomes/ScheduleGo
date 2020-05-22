using System;
using System.Collections.Generic;
using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities
{
	public class Course : Entity
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public int StudentsCount { get; private set; }
		public ClassroomType ClassroomTypeNeeded { get; private set; }
		public TimeSpan WeeklyWorkload { get; private set; }
		public List<Tag> CategoryTags { get; private set; }
		public List<TimePeriod> AvailablePeriods { get; private set; }

		public bool IsAvailable(TimePeriod timePeriod) => AvailablePeriods.Contains(timePeriod);
	}
}