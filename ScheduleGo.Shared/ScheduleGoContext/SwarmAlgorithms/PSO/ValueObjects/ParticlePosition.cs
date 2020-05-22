using System;
using System.Collections.Generic;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts;

namespace ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.ValueObjects
{
	public class ParticlePosition<TPositionType> where TPositionType : class, IPositionType, new()
	{
		private ParticlePosition(ParticlePosition<TPositionType> particle)
		{
			Fitness = particle.Fitness;
			Position = particle.Position.Clone() as TPositionType;
		}

		public ParticlePosition(int dimentions)
		{
			Position = new TPositionType().Build(dimentions) as TPositionType;
			Fitness = Position.CalculateFitness();
		}

		public double Fitness { get; protected set; }
		public TPositionType Position { get; protected set; }

		public void UpdateFitness() => Fitness = Position.CalculateFitness();

		public ParticlePosition<TPositionType> Clone() => new ParticlePosition<TPositionType>(this);

		public IPositionTypeEntry this[int index] { get => Position[index]; }
		public IEnumerator<IPositionTypeEntry> GetEnumerator() => Position.GetEnumerator();

		public static explicit operator double(ParticlePosition<TPositionType> particleValue) => particleValue.Fitness;

		internal void Update<TVelocityType>(ParticleVelocity<TVelocityType> velocity) where TVelocityType : class, IVelocityType, new()
		{
			for (int index = 0; index < Position.Length; index++)
				Position.Update(index, Position[index] + velocity[index].ToDouble());
		}
	}
}