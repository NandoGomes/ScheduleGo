using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ScheduleGo.Domain.ScheduleGoContext.Entities;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts;

namespace ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO
{
    public class PositionType : IPositionType
    {
        private IPositionTypeEntry[] _schedules;

        public IPositionTypeEntry this[int index] => _schedules[index];

        public int Length => _schedules.Length;

        public IPositionType Build(int dimentions, params object[] args)
        {
            IEnumerable<Teacher> teachers = args[0] as IEnumerable<Teacher>;
            IEnumerable<Course> courses = args[1] as IEnumerable<Course>;
            IEnumerable<TimePeriod> timePeriods = args[2] as IEnumerable<TimePeriod>;
            IEnumerable<Classroom> classrooms = args[3] as IEnumerable<Classroom>;

            List<IPositionTypeEntry> schedules = new List<IPositionTypeEntry>(dimentions);

            foreach (Teacher teacher in teachers)
                foreach (TimePeriod timePeriod in timePeriods)
                    schedules.Add(new PositionTypeEntry(courses, classrooms).Initialize(teacher, timePeriod));

            _schedules = schedules.ToArray();

            return this;
        }

        public double CalculateFitness()
        {
            double fitness = 0;

            fitness = _schedules.Sum(schedule => schedule.ToDouble());

            return fitness;
        }

        public IPositionType Clone() => new PositionType { _schedules = _schedules.Select(entry => entry.Clone()).ToArray() };

        public IEnumerator<IPositionTypeEntry> GetEnumerator() => _schedules.GetEnumerator() as IEnumerator<IPositionTypeEntry>;


        public void Update(int index, IVelocityTypeEntry velocity) => _schedules[index].Update(velocity);

        public override string ToString() => JsonSerializer.Serialize(_schedules);
    }
}