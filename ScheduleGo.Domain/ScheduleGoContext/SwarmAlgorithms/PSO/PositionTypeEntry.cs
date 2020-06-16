using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ScheduleGo.Domain.ScheduleGoContext.Entities;
using ScheduleGo.Domain.ScheduleGoContext.Enums;
using ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO.Enums;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts;

namespace ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO
{
	public class PositionTypeEntry : IPositionTypeEntry
	{
		private readonly List<Course> _courses;
		private readonly List<Classroom> _classrooms;

		public PositionTypeEntry(EWeekDay weekDay,
								 List<Course> courses,
								 List<Classroom> classrooms)
		{
			WeekDay = weekDay;
			_courses = courses;
			_classrooms = classrooms;
		}

		public EWeekDay WeekDay { get; private set; }
		public Teacher Teacher { get; private set; }
		public TimePeriod TimePeriod { get; private set; }
		public Course Course { get; private set; }
		public Classroom Classroom { get; private set; }
		public int CourseIndex { get; private set; }
		public int ClassroomIndex { get; private set; }

		public IPositionTypeEntry Initialize(params object[] args)
		{
			Teacher = args[0] as Teacher;
			TimePeriod = args[1] as TimePeriod;

			Randomize();

			return this;
		}

		public double ToDouble()
		{
			double value = 0;

			if (Course != null)
			{
				/*Teacher Must Be qualified for this course*/
				if (!Teacher.IsQualified(Course))
					value += (double)EValidationCosts.UltimatePenalty;

				/*Teacher prefers this course*/
				if (!Teacher.Prefers(Course))
					value += (double)EValidationCosts.SmallPenalty;

				/*Teacher prefers this time*/
				if (!Teacher.Prefers(TimePeriod))
					value += (double)EValidationCosts.SmallPenalty;

				/*Course Must Be availble at this time*/
				if (!Course.IsAvailable(TimePeriod))
					value += (double)EValidationCosts.MediumPenalty;

				/*Classroom must be large enought for all students*/
				if (Course.StudentsCount > Classroom.Capacity)
					value += (double)EValidationCosts.GravePenalty;

				else if (Course.StudentsCount * 1.25 < Classroom.Capacity)
					value += (double)EValidationCosts.MediumPenalty;

				/*Classroom Must Be available at the required time*/
				if (!Classroom.IsAvailable(TimePeriod))
					value += (double)EValidationCosts.SmallPenalty;
			}

			else
				value += (double)EValidationCosts.MediumPenalty;

			return value;
		}

		public void Randomize()
		{
			Random random = new Random();

			CourseIndex = random.Next(_courses.Count());
			ClassroomIndex = random.Next(_classrooms.Count());

			_fetchCourseAndClassroom();
		}

		internal void Reset()
		{
			CourseIndex = 0;
			ClassroomIndex = 0;

			_fetchCourseAndClassroom();
		}

		public IPositionTypeEntry Clone() => new PositionTypeEntry(WeekDay, _courses, _classrooms)
		{
			Teacher = Teacher,
			Course = Course,
			TimePeriod = TimePeriod,
			Classroom = Classroom,
			CourseIndex = CourseIndex,
			ClassroomIndex = ClassroomIndex
		};

		public void Update(IVelocityTypeEntry velocity)
		{
			CourseIndex = (velocity as VelocityTypeEntry).GetNewCourseIndex(CourseIndex);
			ClassroomIndex = (velocity as VelocityTypeEntry).GetNewClassroomIndex(ClassroomIndex);

			_fetchCourseAndClassroom();
		}

		public void UpdateCourse(Course course)
		{
			Course = course;
			CourseIndex = _courses.IndexOf(course) + 1;
		}

		public void UpdateClassroom(Classroom classroom)
		{
			Classroom = classroom;
			ClassroomIndex = _classrooms.IndexOf(classroom) + 1;
		}

		private void _fetchCourseAndClassroom()
		{
			Course = _courses.ElementAtOrDefault(CourseIndex - 1);
			Classroom = _classrooms.ElementAtOrDefault(ClassroomIndex - 1);

			if (Classroom == null)
				Course = null;

			else if (Course != null && !Course.NeededClassroomType.Equals(Classroom))
			{
				int newClassroomIndex = 0;

				do
					newClassroomIndex++;
				while (newClassroomIndex < _classrooms.Count && !Course.NeededClassroomType.Equals(_classrooms[newClassroomIndex].ClassroomType));

				Classroom = _classrooms[newClassroomIndex];
				ClassroomIndex = newClassroomIndex + 1;
			}
		}

		public override string ToString() => JsonSerializer.Serialize(new
		{
			Teacher = Teacher.Name,
			Course = Course.Name,
			Classroom = Classroom.Description,
			TeacherQualified = Teacher.IsQualified(Course),
			PrefersCourse = Teacher.Prefers(Course),
			TeacherAvailable = Teacher.IsAvailable(TimePeriod),
			PrefersTime = Teacher.Prefers(TimePeriod),
			NeededClassroomType = Course.NeededClassroomTypeId == Classroom.ClassroomTypeId,
			CourseAvailable = Course.IsAvailable(TimePeriod),
			ClassroomAvailable = Classroom.IsAvailable(TimePeriod),
			ClassroomSizeEnought = Course.StudentsCount <= Classroom.Capacity,
			CourseId = CourseIndex,
			ClassroomId = ClassroomIndex,
		});
	}
}