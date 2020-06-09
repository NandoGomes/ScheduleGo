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

		public IVelocityType Build(int dimentions, object[] arguments)
		{
			_values = new VelocityTypeEntry[dimentions];

			for (int index = 0; index < dimentions; index++)
				_values[index] = new VelocityTypeEntry().Initialize(arguments[0] as double[], arguments[1] as double[]);

			return this;
		}

		public IVelocityType Clone() => new VelocityType { _values = _values.Select(entry => entry.Clone()).ToArray() };

		public IEnumerator<IVelocityTypeEntry> GetEnumerator() => _values.GetEnumerator() as IEnumerator<IVelocityTypeEntry>;

		public void Update(double weight,
						   double particleWeight,
						   double swarmWeight,
						   IPositionType position,
						   IPositionType bestPosition,
						   IPositionType swarmBestPosition)
		{
			for (int index = 0; index < _values.Length; index++)
				_values[index].Update(weight,
									  particleWeight,
									  swarmWeight,
									  position[index],
									  bestPosition[index],
									  swarmBestPosition[index]);
		}

		public override string ToString() => JsonSerializer.Serialize(_values);
	}
}