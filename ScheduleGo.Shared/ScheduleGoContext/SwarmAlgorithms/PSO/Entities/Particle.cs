using System;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.ValueObjects;

namespace ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Entities
{
	public class Particle<TPositionType, TVelocityType> where TPositionType : class, IPositionType, new() where TVelocityType : class, IVelocityType, new()
	{
		public Particle(int dimentions,
				  double weight)
		{
			Random random = new Random();

			Weight = weight;

			Position = new ParticlePosition<TPositionType>(dimentions);
			Velocity = new ParticleVelocity<TVelocityType>(dimentions);

			BestPosition = Position.Clone();
		}

		public double Weight { get; private set; }
		public ParticleVelocity<TVelocityType> Velocity { get; private set; }
		public ParticlePosition<TPositionType> Position { get; private set; }
		public ParticlePosition<TPositionType> BestPosition { get; private set; }

		public void Run(Swarm<TPositionType, TVelocityType> swarm)
		{
			Velocity.Update<TPositionType>(swarm.Weight,
								  Weight,
								  swarm.GlobalWeight,
								  Position,
								  BestPosition,
								  swarm.BestPosition);

			Position.Update<TVelocityType>(Velocity);

			if (_updateFitness())
				swarm.UpdateBestPosition(Position);
		}

		private bool _updateFitness()
		{
			Position.UpdateFitness();

			bool updateBest = Position.Fitness < BestPosition.Fitness;

			if (updateBest)
				BestPosition = Position.Clone();

			return updateBest;
		}
	}
}