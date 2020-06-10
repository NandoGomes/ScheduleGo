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
		public int CourseId { get; private set; }
		public int ClassroomId { get; private set; }

		public IPositionTypeEntry Initialize(params object[] args)
		{
			Teacher = args[0] as Teacher;
			TimePeriod = args[1] as TimePeriod;

			Random random = new Random();

			CourseId = random.Next(_courses.Count());
			ClassroomId = random.Next(_classrooms.Count());

			_fetchCourseAndClassroom();

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

				/*Teacher Must Be available*/
				if (!Teacher.IsAvailable(TimePeriod))
					value += (double)EValidationCosts.MediumPenalty;

				/*Teacher prefers this time*/
				if (!Teacher.Prefers(TimePeriod))
					value += (double)EValidationCosts.SmallPenalty;

				/*Course Must Be availble at this time*/
				if (!Course.IsAvailable(TimePeriod))
					value += (double)EValidationCosts.MediumPenalty;

				if (Classroom != null)
				{
					/*Classroom must be large enought for all students*/
					if (Course.StudentsCount > Classroom.Capacity)
						value += (double)EValidationCosts.MegaPenalty;

					/*Choosen classroom must have the required type by the course*/
					if (!Classroom.ClassroomType.Equals(Course.NeededClassroomType))
						value += (double)EValidationCosts.MegaPenalty;

					/*Classroom Must Be available at the required time*/
					if (!Classroom.IsAvailable(TimePeriod))
						value += (double)EValidationCosts.MediumPenalty;
				}

				else
					value += (double)EValidationCosts.UltimatePenalty;
			}

			else
				value = (double)EValidationCosts.SmallPenalty;

			return value;
		}

		internal void Reset()
		{
			CourseId = 0;
			ClassroomId = 0;

			_fetchCourseAndClassroom();
		}

		public IPositionTypeEntry Clone() => new PositionTypeEntry(WeekDay, _courses, _classrooms)
		{
			Teacher = Teacher,
			Course = Course,
			TimePeriod = TimePeriod,
			Classroom = Classroom
		};

		public void Update(IVelocityTypeEntry velocity)
		{
			CourseId = (velocity as VelocityTypeEntry).GetNewCouseId(CourseId);
			ClassroomId = (velocity as VelocityTypeEntry).GetNewClassroomId(ClassroomId);

			_fetchCourseAndClassroom();
		}

		private void _fetchCourseAndClassroom()
		{
			Course = _courses.ElementAtOrDefault(CourseId - 1);
			Classroom = _classrooms.ElementAtOrDefault(ClassroomId - 1);
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
			CourseId = CourseId,
			ClassroomId = ClassroomId,
		});
	}
}