using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ScheduleGo.Domain.ScheduleGoContext.Entities;
using ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO.Validations;
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

		public PositionType() { }
		private PositionType(PositionType positionType)
		{
			positionType.CalculateFitness();

			CoursesWorkload = positionType.CoursesWorkload;
			CourseAssignedTeachers = positionType.CourseAssignedTeachers;
			TeacherAssignedCourses = positionType.TeacherAssignedCourses;
			ClassroomAssignedTimePeriods = positionType.ClassroomAssignedTimePeriods;

			_schedules = positionType._schedules.Select(entry => entry.Clone()).ToArray();
			_teachers = positionType._teachers;
			_courses = positionType._courses;
			_timePeriods = positionType._timePeriods;
			_classrooms = positionType._classrooms;
			_teacherMaxAssignedCourses = positionType._teacherMaxAssignedCourses;
		}

		public Dictionary<Course, TimeSpan> CoursesWorkload { get; private set; }
		public Dictionary<Course, Teacher> CourseAssignedTeachers { get; private set; }
		public Dictionary<Teacher, HashSet<Course>> TeacherAssignedCourses { get; private set; }
		public Dictionary<Classroom, HashSet<TimePeriod>> ClassroomAssignedTimePeriods { get; private set; }

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
				foreach (TimePeriod timePeriod in teacher.AvailablePeriods)
					schedules.Add(new PositionTypeEntry(timePeriod.WeekDay, _courses, _classrooms).Initialize(teacher, timePeriod));

			_schedules = schedules.ToArray();

			return this;
		}

		public void BuildControls()
		{
			CoursesWorkload = _courses.ToDictionary(course => course, course => new TimeSpan());
			CourseAssignedTeachers = _courses.ToDictionary(course => course, course => null as Teacher);
			TeacherAssignedCourses = _teachers.ToDictionary(teacher => teacher, teacher => new HashSet<Course>(_courses.Count));
			ClassroomAssignedTimePeriods = _classrooms.ToDictionary(classroom => classroom, classroom => new HashSet<TimePeriod>(_timePeriods.Count));
		}

		public double CalculateFitness()
		{
			double fitness = 0;

			BuildControls();

			foreach (KeyValuePair<PositionTypeEntry, double> calculatedSchedule in _schedules.Select(schedule => new KeyValuePair<PositionTypeEntry, double>((PositionTypeEntry)schedule, schedule.ToDouble()))
																					.OrderBy(calculatedSchedule => calculatedSchedule.Value))
			{
				PositionTypeEntry schedule = calculatedSchedule.Key;
				double scheduleFitness = calculatedSchedule.Value;
				Teacher teacher = schedule.Teacher;
				Course course = schedule.Course;
				TimePeriod timePeriod = schedule.TimePeriod;
				Classroom classroom = schedule.Classroom;

				if (course != null)
				{
					if (_assignCourse(teacher, course))
					{
						fitness += scheduleFitness;
						_increaseCourseWorkLoad(course, timePeriod);

						if (!_assignClassroom(schedule))
							fitness += (double)ValidationCosts.UltimatePenalty;
					}

					else
						schedule.Reset();
				}

				else
					fitness += scheduleFitness;
			}

			Dictionary<Course, Teacher> coursesToBeAssigned = new Dictionary<Course, Teacher>();

			foreach (Course course in CourseAssignedTeachers.Where(pair => pair.Value == null).Select(pair => pair.Key))
			{
				Teacher availableTeacher = TeacherAssignedCourses.Where(pair => pair.Value.Count < _teacherMaxAssignedCourses && pair.Key.IsQualified(course)).Select(pair => pair.Key).FirstOrDefault();

				if (availableTeacher != null)
				{
					PositionTypeEntry availableSchedule = (PositionTypeEntry)_schedules.Where(schedule => ((PositionTypeEntry)schedule).Teacher == availableTeacher && ((PositionTypeEntry)schedule).Course == null).FirstOrDefault();

					if (availableSchedule != null)
					{
						availableSchedule.UpdateCourse(course);
						_assignClassroom(availableSchedule);
						coursesToBeAssigned.Add(course, availableTeacher);
					}
				}
			}

			foreach (KeyValuePair<Course, Teacher> courseToBeAssigned in coursesToBeAssigned)
				_assignCourse(courseToBeAssigned.Value, courseToBeAssigned.Key);

			/*Penalty based on the course's workload*/
			foreach (KeyValuePair<Course, TimeSpan> courseWorkload in CoursesWorkload)
			{
				/*If course was not assigned at all*/
				if (courseWorkload.Value.TotalMilliseconds == 0)
					fitness += (double)ValidationCosts.UltimatePenalty * courseWorkload.Key.WeeklyWorkload.TotalMinutes;

				/*If workload was not met*/
				else if (courseWorkload.Key.WeeklyWorkload > courseWorkload.Value)
					fitness += (double)ValidationCosts.SmallPenalty * ((courseWorkload.Key.WeeklyWorkload.TotalMinutes - courseWorkload.Value.TotalMinutes) / 15);
			}

			int unassignedTeachersCount = TeacherAssignedCourses.Where(pair => !pair.Value.Any()).Count();

			for (int index = 0; index < unassignedTeachersCount; index++)
				fitness += (double)ValidationCosts.MegaPenalty;

			return fitness;
		}

		public IPositionType Clone() => new PositionType(this);

		public IEnumerator<IPositionTypeEntry> GetEnumerator() => _schedules.GetEnumerator() as IEnumerator<IPositionTypeEntry>;

		public void Update(int index, IVelocityTypeEntry velocity) => _schedules[index].Update(velocity);

		public IEnumerable<PositionTypeEntry> GetFinalSchedule() => _schedules.Where(entry => (entry as PositionTypeEntry).Course != null).Select(entry => entry as PositionTypeEntry);

		private bool _assignCourse(Teacher teacher, Course course)
		{
			bool result = false;

			if (!_courseIsAssignedToAnotherTeacher(course, teacher) && !_courseWorkloadAlreadyReached(course))
			{
				if (TeacherAssignedCourses[teacher].Contains(course))
					result = true;

				else if (!_teacherAssignedTooManyCourses(teacher))
				{
					TeacherAssignedCourses[teacher].Add(course);
					CourseAssignedTeachers[course] = teacher;

					result = true;
				}
			}

			return result;
		}

		private bool _assignClassroom(PositionTypeEntry schedule)
		{
			bool result = true;

			if (schedule.Classroom == null || !ClassroomAssignedTimePeriods[schedule.Classroom].Add(schedule.TimePeriod))
			{
				Classroom classroom = ClassroomAssignedTimePeriods.Where(pair => !pair.Value.Contains(schedule.TimePeriod)
																	 && pair.Key.Capacity >= schedule.Course.StudentsCount)
					.OrderByDescending(pair => pair.Key.ClassroomType.Equals(schedule.Course.NeededClassroomType))
					.Select(pair => pair.Key)
					.FirstOrDefault();

				if (classroom != null)
					schedule.UpdateClassroom(classroom);

				else
					result = false;
			}

			return result;
		}

		private bool _teacherAssignedTooManyCourses(Teacher teacher) => TeacherAssignedCourses[teacher].Count >= _teacherMaxAssignedCourses;
		private bool _courseIsAssignedToAnotherTeacher(Course course, Teacher teacher) => CourseAssignedTeachers[course] != null && CourseAssignedTeachers[course] != teacher;
		private bool _courseWorkloadAlreadyReached(Course course) => CoursesWorkload[course] >= course.WeeklyWorkload;
		private void _increaseCourseWorkLoad(Course course, TimePeriod timePeriod) => CoursesWorkload[course] += timePeriod.Duration;
	}
}