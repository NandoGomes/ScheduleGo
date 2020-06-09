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
		private int _teacherMaxAssignedCourses;

		public IPositionTypeEntry this[int index] => _schedules[index];
		public IPositionTypeEntry[] Schedule => _schedules.Where(entry => (entry as PositionTypeEntry).Course != null && (entry as PositionTypeEntry).Classroom != null).ToArray();

		public int Length => _schedules.Length;

		public IPositionType Build(int dimentions, params object[] arguments)
		{
			_teacherMaxAssignedCourses = (int)arguments[4];

			List<Teacher> teachers = arguments[0] as List<Teacher>;
			List<Course> courses = arguments[1] as List<Course>;
			List<TimePeriod> timePeriods = arguments[2] as List<TimePeriod>;
			List<Classroom> classrooms = arguments[3] as List<Classroom>;

			List<IPositionTypeEntry> schedules = new List<IPositionTypeEntry>(dimentions);

			foreach (Teacher teacher in teachers)
				foreach (TimePeriod timePeriod in timePeriods)
					schedules.Add(new PositionTypeEntry(timePeriod.WeekDay, courses, classrooms).Initialize(teacher, timePeriod));

			_schedules = schedules.ToArray();

			return this;
		}

		public double CalculateFitness()
		{
			double fitness = 0;

			Dictionary<Course, TimeSpan> coursesWorkload = new Dictionary<Course, TimeSpan>();
			Dictionary<Course, Teacher> assignedCourses = new Dictionary<Course, Teacher>();
			Dictionary<Teacher, HashSet<Course>> assignedTeachers = new Dictionary<Teacher, HashSet<Course>>();
			Dictionary<Classroom, HashSet<TimePeriod>> assignedClassrooms = new Dictionary<Classroom, HashSet<TimePeriod>>();

			foreach (KeyValuePair<PositionTypeEntry, double> calculatedSchedule in _schedules.Select(schedule => new KeyValuePair<PositionTypeEntry, double>((PositionTypeEntry)schedule, schedule.ToDouble()))
															   .OrderByDescending(calculatedSchedule => calculatedSchedule.Value))
			{
				fitness += calculatedSchedule.Value;

				if (!assignedTeachers.ContainsKey(calculatedSchedule.Key.Teacher))
					assignedTeachers.Add(calculatedSchedule.Key.Teacher, new HashSet<Course> { calculatedSchedule.Key.Course });

				/*Teacher is assigned to too many courses*/
				else if (assignedTeachers[calculatedSchedule.Key.Teacher].Add(calculatedSchedule.Key.Course)
					&& assignedTeachers[calculatedSchedule.Key.Teacher].Count > _teacherMaxAssignedCourses)
					fitness += (double)EValidationCosts.GravePenalty;

				if (!coursesWorkload.ContainsKey(calculatedSchedule.Key.Course))
				{
					coursesWorkload.Add(calculatedSchedule.Key.Course, calculatedSchedule.Key.TimePeriod.Duration);
					assignedCourses.Add(calculatedSchedule.Key.Course, calculatedSchedule.Key.Teacher);
				}

				/*Course already assigned to a teacher*/
				else if (assignedCourses[calculatedSchedule.Key.Course] != calculatedSchedule.Key.Teacher)
					fitness += (double)EValidationCosts.GravePenalty;

				/*Classroom is already in use*/
				if (!assignedClassrooms.ContainsKey(calculatedSchedule.Key.Classroom))
					assignedClassrooms.Add(calculatedSchedule.Key.Classroom, new HashSet<TimePeriod> { calculatedSchedule.Key.TimePeriod });

				else if (!assignedClassrooms[calculatedSchedule.Key.Classroom].Add(calculatedSchedule.Key.TimePeriod))
					fitness += (double)EValidationCosts.GravePenalty;
			}

			/*Penalty based on the course's workload*/
			foreach (KeyValuePair<Course, TimeSpan> courseWorkload in coursesWorkload)
			{
				/*If workload was not met*/
				if (courseWorkload.Key.WeeklyWorkload < courseWorkload.Value)
					fitness += (Double)EValidationCosts.GravePenalty;

				/*If way past required workload*/
				else if (courseWorkload.Key.WeeklyWorkload.TotalMilliseconds * 1.25 > courseWorkload.Value.TotalMilliseconds)
					fitness += (Double)EValidationCosts.MediumPenalty;
			}

			return fitness;
		}

		public IPositionType Clone() => new PositionType { _schedules = _schedules.Select(entry => entry.Clone()).ToArray() };

		public IEnumerator<IPositionTypeEntry> GetEnumerator() => _schedules.GetEnumerator() as IEnumerator<IPositionTypeEntry>;

		public void Update(int index, IVelocityTypeEntry velocity) => _schedules[index].Update(velocity);

		public override string ToString() => JsonSerializer.Serialize(Schedule);
	}
}