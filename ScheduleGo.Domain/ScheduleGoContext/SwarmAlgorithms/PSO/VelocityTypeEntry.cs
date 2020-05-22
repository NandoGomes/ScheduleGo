using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts;

namespace ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO
{
	public class VelocityTypeEntry : IVelocityTypeEntry
	{
		public double Value { get; private set; }

		public void Initialize() => Value = double.MaxValue;

		public double ToDouble() => Value;

		public VelocityTypeEntry Clone() => new VelocityTypeEntry { Value = Value };

		public static implicit operator VelocityTypeEntry(double value) => new VelocityTypeEntry { Value = value };
		public static implicit operator double(VelocityTypeEntry entry) => entry.Value;
	}
}