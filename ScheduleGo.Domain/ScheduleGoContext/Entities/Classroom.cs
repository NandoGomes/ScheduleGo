using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleGo.Domain.ScheduleGoContext.Entities.Links;
using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities
{
	public class Classroom : Entity
	{
		public string Description { get; private set; }
		public int Capacity { get; private set; }
		public Guid ClassroomTypeId { get; private set; }

		public virtual ClassroomType ClassroomType { get; private set; }
		public virtual IEnumerable<ClassroomTag> CategoryTags { get; private set; }
		public virtual IEnumerable<ClassroomTimePeriod> AvailablePeriods { get; private set; }

		public bool IsAvailable(TimePeriod timePeriod) => AvailablePeriods.Where(availablePeriod => (TimePeriod)availablePeriod == timePeriod).Any();
	}
}