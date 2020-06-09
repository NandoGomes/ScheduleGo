using System;
using System.Collections.Generic;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts;

namespace ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.ValueObjects
{
	public class ParticleVelocity<TVelocityType> where TVelocityType : class, IVelocityType, new()
	{
		private ParticleVelocity(ParticleVelocity<TVelocityType> velocity) => Velocity = velocity.Velocity.Clone() as TVelocityType;

		public ParticleVelocity(int dimentions, object[] arguments) => Velocity = new TVelocityType().Build(dimentions, arguments) as TVelocityType;

		public TVelocityType Velocity { get; private set; }

		public ParticleVelocity<TVelocityType> Clone() => new ParticleVelocity<TVelocityType>(this);

		public IVelocityTypeEntry this[int index] { get => Velocity[index]; }
		public IEnumerator<IVelocityTypeEntry> GetEnumerator() => Velocity.GetEnumerator();

		public void Update<TPositionType>(double weight,
										  double particleWeight,
										  double swarmWeight,
										  ParticlePosition<TPositionType> position,
										  ParticlePosition<TPositionType> bestPosition,
										  ParticlePosition<TPositionType> swarmBestPosition) where TPositionType : class, IPositionType, new() => Velocity.Update(weight,
																																								  particleWeight,
																																								  swarmWeight,
																																								  position.Position,
																																								  bestPosition.Position,
																																								  swarmBestPosition.Position);
	}
}