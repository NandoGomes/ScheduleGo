using System;
using ScheduleGo.Domain.ScheduleGoContext.Enums;
using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities
{
	public class TimePeriod : Entity
	{
		public string Description { get; private set; }
		public TimeSpan Start { get; private set; }
		public TimeSpan End { get; private set; }
		public EWeekDay WeekDay { get; private set; }

		public TimeSpan Duration => End.Subtract(Start);
	}
}