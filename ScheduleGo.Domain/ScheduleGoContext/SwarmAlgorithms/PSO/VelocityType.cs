using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts;

namespace ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO
{
	public class VelocityType : IVelocityType
	{
		private IVelocityTypeEntry[] _values;

		public IVelocityTypeEntry this[int index] => _values[index];

		public int Length => _values.Length;

		public IVelocityType Build(int dimentions)
		{
			Random random = new Random();
			_values = new VelocityTypeEntry[dimentions];

			double absolute = Math.Abs(20000.0);

			for (int index = 0; index < dimentions; index++)
				_values[index] = (VelocityTypeEntry)((absolute * 2) * random.NextDouble() - absolute);

			return this;
		}

		public IVelocityType Clone() => new VelocityType { _values = _values.Select(entry => ((VelocityTypeEntry)entry).Clone()).ToArray() };

		public IEnumerator<IVelocityTypeEntry> GetEnumerator() => _values.GetEnumerator() as IEnumerator<IVelocityTypeEntry>;

		public void Update(int index, double value) => _values[index] = (VelocityTypeEntry)value;
		public override string ToString() => JsonSerializer.Serialize(_values);
	}
}