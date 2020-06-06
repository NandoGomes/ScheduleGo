using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleGo.Domain.ScheduleGoContext.Entities;
using ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO.Enums;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts;

namespace ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO
{
	public class PositionTypeEntry : IPositionTypeEntry
	{
		private readonly IEnumerable<Course> _courses;
		private readonly IEnumerable<Classroom> _classrooms;

		public PositionTypeEntry(IEnumerable<Course> courses,
								 IEnumerable<Classroom> classrooms)
		{
			_courses = courses;
			_classrooms = classrooms;
		}

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

			Course = _courses.ElementAt(CourseId);
			Classroom = _classrooms.ElementAt(ClassroomId);

			return this;
		}

		public double ToDouble()
		{
			double value = 0;

			if (!Teacher.IsQualified(Course))
				value += (double)EValidationCosts.MediumPenalty;

			if (Teacher.Prefers(Course))
				value += (double)EValidationCosts.SmallBonus;

			if (!Teacher.IsAvailable(TimePeriod))
				value += (double)EValidationCosts.GravePenalty;

			if (Teacher.Prefers(TimePeriod))
				value += (double)EValidationCosts.MediumBonus;

			if (!Course.IsAvailable(TimePeriod))
				value += (double)EValidationCosts.GravePenalty;

			if (Course.StudentsCount > Classroom.Capacity)
				value += (double)EValidationCosts.GravePenalty;

			if (!Classroom.ClassroomType.Equals(Course.NeededClassroomType))
				value += (double)EValidationCosts.GravePenalty;

			if (!Classroom.IsAvailable(TimePeriod))
				value += (double)EValidationCosts.GravePenalty;

			return value;
		}

		public IPositionTypeEntry Clone() => new PositionTypeEntry(_courses, _classrooms)
		{
			Teacher = Teacher,
			Course = Course,
			TimePeriod = TimePeriod,
			Classroom = Classroom
		};

		public void Update(IVelocityTypeEntry velocity)
		{
			CourseId += (velocity as VelocityTypeEntry).CourseVelocity;
			ClassroomId += (velocity as VelocityTypeEntry).ClassroomVelocity;

			Course = _courses.ElementAt(CourseId);
			Classroom = _classrooms.ElementAt(ClassroomId);
		}
	}
}