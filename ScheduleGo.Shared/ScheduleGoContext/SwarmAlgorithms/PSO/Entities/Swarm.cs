using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.ValueObjects;

namespace ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Entities
{
    public class Swarm<TPositionType, TVelocityType> : IList<Particle<TPositionType, TVelocityType>> where TPositionType : class, IPositionType, new() where TVelocityType : class, IVelocityType, new()
    {
        private List<Particle<TPositionType, TVelocityType>> _particles;
        private double[] _optimalFitnessRange = new double[] { 0, 0 };

        public Swarm(int lifetime,
               double weight,
               double globalWeight)
        {
            Lifetime = lifetime;
            Weight = weight;
            GlobalWeight = globalWeight;

            BestPosition = null;
        }

        public ParticlePosition<TPositionType> BestPosition { get; private set; }
        public int Lifetime { get; private set; }
        public double Weight { get; private set; }
        public double GlobalWeight { get; private set; }
        public int Iterations { get; private set; }

        public Swarm<TPositionType, TVelocityType> OptimalFitnessRange(double start, double end)
        {
            _optimalFitnessRange = new double[] { start, end };

            return this;
        }

        public Swarm<TPositionType, TVelocityType> Build(int size,
                                                   int dimentions,
                                                   double particleWeight,
                                                   double[] velocityMinValues,
                                                   double[] velocityMaxValues,
                                                   params object[] args)
        {
            _particles = new List<Particle<TPositionType, TVelocityType>>(size);

            for (int index = 0; index < size; index++)
            {
                _particles.Add(new Particle<TPositionType, TVelocityType>(dimentions,
                                                                          particleWeight,
                                                                          velocityMinValues,
                                                                          velocityMaxValues,
                                                                          args));
                UpdateBestPosition(_particles[index].Position);
            }

            return this;
        }

        public Swarm<TPositionType, TVelocityType> Process()
        {
            int iterationCounter = 0;

            for (iterationCounter = 0; iterationCounter < Lifetime && !OptimalFitnessFound; iterationCounter++)
                for (int index = 0; index < _particles.Count && !OptimalFitnessFound; index++)
                    _particles[index].Run(this);

            Iterations = iterationCounter;

            return this;
        }

        public bool OptimalFitnessFound { get => BestPosition.Fitness > _optimalFitnessRange[0] && BestPosition.Fitness < _optimalFitnessRange[1]; }

        public void UpdateBestPosition(ParticlePosition<TPositionType> particlePosition)
        {
            if (BestPosition == null || particlePosition.Fitness < BestPosition.Fitness)
                BestPosition = particlePosition.Clone();
        }

        public Particle<TPositionType, TVelocityType> this[int index] { get => _particles[index]; set => _particles[index] = value; }

        public int Count => _particles.Count;

        public bool IsReadOnly => true;

        public void Add(Particle<TPositionType, TVelocityType> item) => _particles.Add(item);

        public void Clear() => _particles.Clear();

        public bool Contains(Particle<TPositionType, TVelocityType> item) => _particles.Contains(item);

        public void CopyTo(Particle<TPositionType, TVelocityType>[] array, int arrayIndex) => _particles.CopyTo(array, arrayIndex);

        public IEnumerator<Particle<TPositionType, TVelocityType>> GetEnumerator() => _particles.GetEnumerator();

        public int IndexOf(Particle<TPositionType, TVelocityType> item) => _particles.IndexOf(item);

        public void Insert(int index, Particle<TPositionType, TVelocityType> item) => _particles.Insert(index, item);

        public bool Remove(Particle<TPositionType, TVelocityType> item) => _particles.Remove(item);

        public void RemoveAt(int index) => _particles.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public static explicit operator ReadOnlyCollection<Particle<TPositionType, TVelocityType>>(Swarm<TPositionType, TVelocityType> swarm) => swarm._particles.AsReadOnly();
    }
}