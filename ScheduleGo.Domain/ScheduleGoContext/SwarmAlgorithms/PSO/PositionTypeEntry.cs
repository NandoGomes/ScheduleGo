using ScheduleGo.Domain.ScheduleGoContext.Entities;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts;

namespace ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO
{
	public class PositionTypeEntry : IPositionTypeEntry
	{
		public Teacher Teacher { get; private set; }
		public Course Course { get; private set; }
		public TimePeriod TimePeriod { get; private set; }
		public Classroom Classroom { get; private set; }

		public void Initialize()
		{

		}

		public double ToDouble()
		{
			double value = 0;

			if (!Teacher.IsQualified(Course))
				value += 50;

			if (Teacher.Prefers(Course))
				value -= 25;

			if (!Teacher.IsAvailable(TimePeriod))
				value += 50;

			if (Teacher.Prefers(TimePeriod))
				value -= 25;

			if (!Course.IsAvailable(TimePeriod))
				value += 50;

			if (Course.StudentsCount > Classroom.Capacity)
				value += 50;

			if (Course.ClassroomTypeNeeded != Classroom.ClassroomType)
				value += 50;

			if (!-Classroom.IsAvailable(TimePeriod))
				value += 50;

			return value;
		}

		public PositionTypeEntry Clone() => new PositionTypeEntry
		{
			Teacher = Teacher,
			TimePeriod = TimePeriod,
			Classroom = Classroom,
			Course = Course
		};

		public static implicit operator double(PositionTypeEntry entry) => entry.ToDouble();
	}
}