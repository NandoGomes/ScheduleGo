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

			CourseVelocity = (int)Math.Round((weight * CourseVelocity)
				+ (particleWeight * localRandomization * (((PositionTypeEntry)bestPosition).CourseId - ((PositionTypeEntry)position).CourseId))
				+ (swarmWeight * globalRandomization * (((PositionTypeEntry)swarmBestPosition).CourseId - ((PositionTypeEntry)position).CourseId)));

			ClassroomVelocity = (int)Math.Round((weight * ClassroomVelocity)
				+ (particleWeight * localRandomization * (((PositionTypeEntry)bestPosition).ClassroomId - ((PositionTypeEntry)position).ClassroomId))
				+ (swarmWeight * globalRandomization * (((PositionTypeEntry)swarmBestPosition).ClassroomId - ((PositionTypeEntry)position).ClassroomId)));
		}

		public int GetNewCouseId(int currentCourseId)
		{
			currentCourseId += CourseVelocity;

			if (currentCourseId < _courseMinValue)
				currentCourseId = _courseMinValue;

			else if (currentCourseId > _courseMaxValue)
				currentCourseId = _courseMaxValue;

			return currentCourseId;
		}

		public int GetNewClassroomId(int currentClassroomId)
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