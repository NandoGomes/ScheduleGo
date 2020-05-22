using System;
using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities
{
	public class TimePeriod : Entity
	{
		public string Description { get; private set; }
		public DateTime Start { get; private set; }
		public DateTime End { get; private set; }
	}
}