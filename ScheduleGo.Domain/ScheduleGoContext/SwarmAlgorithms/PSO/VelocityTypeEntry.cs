using System;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts;

namespace ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO
{
	public class VelocityTypeEntry : IVelocityTypeEntry
	{
		private int _courseMinValue;
		private int _courseMaxValue;

		private int _classroomMinValue;
		private int _classroomMaxValue;

		public int CourseVelocity { get; private set; }
		public int ClassroomVelocity { get; private set; }

		public void Update(double weight,
						   double particleWeight,
						   double swarmWeight,
						   IPositionTypeEntry position,
						   IPositionTypeEntry bestPosition,
						   IPositionTypeEntry swarmBestPosition)
		{
			Random random = new Random();

			double localRandomization = random.NextDouble();
			double globalRandomization = random.NextDouble();

			CourseVelocity = (int)Math.Round((weight * CourseVelocity + 1)
				+ (particleWeight * localRandomization * (((PositionTypeEntry)bestPosition).CourseIndex - ((PositionTypeEntry)position).CourseIndex))
				+ (swarmWeight * globalRandomization * (((PositionTypeEntry)swarmBestPosition).CourseIndex - ((PositionTypeEntry)position).CourseIndex)));

			ClassroomVelocity = (int)Math.Round((weight * ClassroomVelocity + 1)
				+ (particleWeight * localRandomization * (((PositionTypeEntry)bestPosition).ClassroomIndex - ((PositionTypeEntry)position).ClassroomIndex))
				+ (swarmWeight * globalRandomization * (((PositionTypeEntry)swarmBestPosition).ClassroomIndex - ((PositionTypeEntry)position).ClassroomIndex)));
		}

		public int GetNewCourseIndex(int currentCourseId)
		{
			currentCourseId += CourseVelocity;

			if (currentCourseId < _courseMinValue)
				currentCourseId = _courseMinValue;

			else if (currentCourseId > _courseMaxValue)
				currentCourseId = _courseMaxValue;

			return currentCourseId;
		}

		public int GetNewClassroomIndex(int currentClassroomId)
		{
			currentClassroomId += ClassroomVelocity;

			if (currentClassroomId < _classroomMinValue)
				currentClassroomId = _classroomMinValue;

			else if (currentClassroomId > _classroomMaxValue)
				currentClassroomId = _classroomMaxValue;

			return currentClassroomId;
		}

		public IVelocityTypeEntry Clone() => new VelocityTypeEntry
		{
			CourseVelocity = CourseVelocity,
			ClassroomVelocity = ClassroomVelocity
		};

		public IVelocityTypeEntry Initialize(double[] minValues, double[] maxValues)
		{
			Random random = new Random();

			_courseMinValue = (int)Math.Round(minValues[0]);
			_courseMaxValue = (int)Math.Round(maxValues[0]);

			_classroomMinValue = (int)Math.Round(minValues[1]);
			_classroomMaxValue = (int)Math.Round(maxValues[1]);

			double courseAbsolute = Math.Abs(_courseMinValue - _courseMinValue);
			double classroomAbsolute = Math.Abs(_classroomMaxValue - _classroomMinValue);

			CourseVelocity = (int)Math.Round((courseAbsolute * 2) * random.NextDouble() - courseAbsolute);
			ClassroomVelocity = (int)Math.Round((classroomAbsolute * 2) * random.NextDouble() - classroomAbsolute);

			return this;
		}
	}
}