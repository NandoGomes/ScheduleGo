using System;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts;

namespace ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO
{
    public class VelocityTypeEntry : IVelocityTypeEntry
    {
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

        public IVelocityTypeEntry Clone() => new VelocityTypeEntry
        {
            CourseVelocity = CourseVelocity,
            ClassroomVelocity = ClassroomVelocity
        };

        public IVelocityTypeEntry Initialize(double[] minValues, double[] maxValues)
        {
            Random random = new Random();

            double courseAbsolute = Math.Abs(maxValues[0] - minValues[0]);
            double classroomAbsolute = Math.Abs(maxValues[0] - minValues[0]);

            CourseVelocity = (int)Math.Round((courseAbsolute * 2) * random.NextDouble() - courseAbsolute);
            ClassroomVelocity = (int)Math.Round((classroomAbsolute * 2) * random.NextDouble() - classroomAbsolute);

            return this;
        }
    }
}