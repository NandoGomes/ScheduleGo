using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts;

namespace ScheduleGo.Engine.SwarmAlgorithms.PSO
{
	public class PositionTypeEntry : IPositionTypeEntry
	{
		public double Value { get; private set; }

		public void Initialize() => Value = double.MaxValue;

		public double ToDouble() => Value;

		public PositionTypeEntry Clone() => new PositionTypeEntry { Value = Value };

		public static implicit operator PositionTypeEntry(double value) => new PositionTypeEntry { Value = value };
		public static implicit operator double(PositionTypeEntry entry) => entry.Value;
	}
}