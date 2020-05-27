using System.Collections.Generic;
using System.Linq;
using ScheduleGo.Domain.ScheduleGoContext.Entities.Links;
using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Entities
{
	public class Teacher : Entity
	{
		public string Name { get; private set; }
		public virtual IEnumerable<TeacherPreferredCourse> PreferredCourses { get; private set; }
		public virtual IEnumerable<TeacherQualifiedCourse> QualifiedCourses { get; private set; }
		public virtual IEnumerable<TeacherPreferredPeriod> PreferredPeriods { get; private set; }
		public virtual IEnumerable<TeacherAvailablePeriod> AvailablePeriods { get; private set; }

		public bool IsQualified(Course course) => QualifiedCourses.Where(qualifiedCourse => (Course)qualifiedCourse == course).Any();
		public bool Prefers(Course course) => PreferredCourses.Where(preferredCourse => (Course)preferredCourse == course).Any();

		public bool IsAvailable(TimePeriod timePeriod) => AvailablePeriods.Where(availablePeriod => (TimePeriod)availablePeriod == timePeriod).Any();
		public bool Prefers(TimePeriod timePeriod) => PreferredPeriods.Where(preferredPeriod => (TimePeriod)preferredPeriod == timePeriod).Any();
	}
}