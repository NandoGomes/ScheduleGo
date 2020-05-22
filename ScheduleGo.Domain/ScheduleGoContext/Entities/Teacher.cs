using System.Collections.Generic;
using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities
{
	public class Teacher : Entity
	{
		public string Name { get; private set; }
		public List<Course> PreferredCourses { get; private set; }
		public List<Course> QualifiedCourses { get; private set; }
		public List<TimePeriod> PreferredPeriods { get; private set; }
		public List<TimePeriod> AvailablePeriods { get; private set; }

		public bool IsQualified(Course course) => QualifiedCourses.Contains(course);
		public bool Prefers(Course course) => PreferredCourses.Contains(course);

		public bool IsAvailable(TimePeriod timePeriod) => AvailablePeriods.Contains(timePeriod);
		public bool Prefers(TimePeriod timePeriod) => PreferredPeriods.Contains(timePeriod);
	}
}