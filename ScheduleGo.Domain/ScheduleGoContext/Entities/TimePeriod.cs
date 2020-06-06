using System;
using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities
{
	public class TimePeriod : Entity
	{
		public string Description { get; private set; }
		public TimeSpan Start { get; private set; }
		public TimeSpan End { get; private set; }
	}
}