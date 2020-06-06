using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleGo.Domain.ScheduleGoContext.Entities.Links;
using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities
{
	public class Course : Entity
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public int StudentsCount { get; private set; }
		public Guid NeededClassroomTypeId { get; private set; }
		public TimeSpan WeeklyWorkload { get; private set; }


		public virtual ClassroomType NeededClassroomType { get; private set; }
		public virtual IEnumerable<CourseTag> CategoryTags { get; private set; }
		public virtual IEnumerable<CourseTimePeriod> AvailablePeriods { get; private set; }

		public bool IsAvailable(TimePeriod timePeriod) => AvailablePeriods.Where(availablePeriod => (TimePeriod)availablePeriod == timePeriod).Any();
	}
}