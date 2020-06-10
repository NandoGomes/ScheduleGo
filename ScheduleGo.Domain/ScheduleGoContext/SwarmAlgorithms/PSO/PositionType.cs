using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ScheduleGo.Domain.ScheduleGoContext.Entities;
using ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO.Enums;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts;

namespace ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO
{
	public class PositionType : IPositionType
	{
		private IPositionTypeEntry[] _schedules;

		private List<Teacher> _teachers;
		private List<Course> _courses;
		private List<TimePeriod> _timePeriods;
		private List<Classroom> _classrooms;

		private int _teacherMaxAssignedCourses;

		public IPositionTypeEntry this[int index] => _schedules[index];

		public int Length => _schedules.Length;

		public IPositionType Build(int dimentions, params object[] arguments)
		{
			_teacherMaxAssignedCourses = (int)arguments[4];

			_teachers = arguments[0] as List<Teacher>;
			_courses = arguments[1] as List<Course>;
			_timePeriods = arguments[2] as List<TimePeriod>;
			_classrooms = arguments[3] as List<Classroom>;

			List<IPositionTypeEntry> schedules = new List<IPositionTypeEntry>(dimentions);

			foreach (Teacher teacher in _teachers)
				foreach (TimePeriod timePeriod in _timePeriods)
					schedules.Add(new PositionTypeEntry(timePeriod.WeekDay, _courses, _classrooms).Initialize(teacher, timePeriod));

			_schedules = schedules.ToArray();

			return this;
		}

		public double CalculateFitness()
		{
			double fitness = 0;

			Dictionary<Course, TimeSpan> coursesWorkload = _courses.ToDictionary(course => course, course => new TimeSpan());

			Dictionary<Course, Teacher> assignedCourses = new Dictionary<Course, Teacher>();

			Dictionary<Teacher, HashSet<Course>> assignedTeachers = _teachers.ToDictionary(teacher => teacher, teacher => new HashSet<Course>(_courses.Count));
			Dictionary<Classroom, HashSet<TimePeriod>> assignedClassrooms = _classrooms.ToDictionary(classroom => classroom, classroom => new HashSet<TimePeriod>(_timePeriods.Count));

			foreach (KeyValuePair<PositionTypeEntry, double> calculatedSchedule in _schedules.Select(schedule => new KeyValuePair<PositionTypeEntry, double>((PositionTypeEntry)schedule, schedule.ToDouble()))
																					.OrderBy(calculatedSchedule => calculatedSchedule.Value))
			{
				if (calculatedSchedule.Key.Course != null && calculatedSchedule.Key.Classroom != null)
				{
					/*Teacher is assigned to too many courses*/
					if (assignedTeachers[calculatedSchedule.Key.Teacher].Add(calculatedSchedule.Key.Course)
						&& assignedTeachers[calculatedSchedule.Key.Teacher].Count > _teacherMaxAssignedCourses)
						calculatedSchedule.Key.Reset();

					else
					{
						fitness += calculatedSchedule.Value;

						if (!assignedCourses.ContainsKey(calculatedSchedule.Key.Course))
						{
							assignedCourses.Add(calculatedSchedule.Key.Course, calculatedSchedule.Key.Teacher);
							coursesWorkload[calculatedSchedule.Key.Course] = calculatedSchedule.Key.TimePeriod.Duration;
						}

						/*Course already assigned to a teacher*/
						else if (assignedCourses[calculatedSchedule.Key.Course] != calculatedSchedule.Key.Teacher)
							fitness += (double)EValidationCosts.GravePenalty;

						else
							coursesWorkload[calculatedSchedule.Key.Course] = calculatedSchedule.Key.TimePeriod.Duration;

						/*Classroom is already in use*/
						if (!assignedClassrooms[calculatedSchedule.Key.Classroom].Add(calculatedSchedule.Key.TimePeriod))
							fitness += (double)EValidationCosts.GravePenalty;
					}
				}

				else
					fitness += calculatedSchedule.Value;
			}

			/*Penalty based on the course's workload*/
			foreach (KeyValuePair<Course, TimeSpan> courseWorkload in coursesWorkload)
			{
				/*If workload was not met*/
				if (courseWorkload.Key.WeeklyWorkload > courseWorkload.Value)
					fitness += (Double)EValidationCosts.GravePenalty * ((courseWorkload.Key.WeeklyWorkload.TotalMinutes - courseWorkload.Value.TotalMinutes) / 10);

				/*If way past required workload*/
				else if (courseWorkload.Key.WeeklyWorkload.TotalMilliseconds * 1.25 < courseWorkload.Value.TotalMilliseconds)
					fitness += (Double)EValidationCosts.SmallPenalty * ((courseWorkload.Value.TotalMilliseconds - courseWorkload.Key.WeeklyWorkload.TotalMilliseconds * 1.25) / 10);
			}

			return fitness;
		}

		public IPositionType Clone() => new PositionType { _schedules = _schedules.Select(entry => entry.Clone()).ToArray() };

		public IEnumerator<IPositionTypeEntry> GetEnumerator() => _schedules.GetEnumerator() as IEnumerator<IPositionTypeEntry>;

		public void Update(int index, IVelocityTypeEntry velocity) => _schedules[index].Update(velocity);

		public IEnumerable<PositionTypeEntry> GetFinalSchedule() => _schedules.Where(entry => (entry as PositionTypeEntry).Course != null).Select(entry => entry as PositionTypeEntry);

		public override string ToString() => JsonSerializer.Serialize(_schedules);
	}
}