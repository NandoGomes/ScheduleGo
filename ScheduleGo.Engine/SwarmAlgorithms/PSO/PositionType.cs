using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts;

namespace ScheduleGo.Engine.SwarmAlgorithms.PSO
{
	public class PositionType : IPositionType
	{
		private IPositionTypeEntry[] _values;

		public IPositionTypeEntry this[int index] => _values[index];

		public int Length => _values.Length;

		public IPositionType Build(int dimentions)
		{
			Random random = new Random();
			_values = new PositionTypeEntry[dimentions];

			for (int index = 0; index < dimentions; index++)
				_values[index] = (PositionTypeEntry)(20000.0 * random.NextDouble() - 10000.0);

			return this;
		}

		public double CalculateFitness() => 3.0 + this[0] * this[0] + this[1] * this[1] + this[2] * this[2];

		public IPositionType Clone() => new PositionType { _values = _values.Select(entry => ((PositionTypeEntry)entry).Clone()).ToArray() };

		public IEnumerator<IPositionTypeEntry> GetEnumerator() => _values.GetEnumerator() as IEnumerator<IPositionTypeEntry>;


		public void Update(int index, double value) => _values[index] = (PositionTypeEntry)value;

		public override string ToString() => JsonSerializer.Serialize(_values);
	}
}