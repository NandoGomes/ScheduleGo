using System.Collections.Generic;

namespace ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts
{
	public interface IVelocityType
	{
		int Length { get; }

		IVelocityType Build(int dimentions);
		IVelocityType Clone();

		void Update(int index, double value);

		IVelocityTypeEntry this[int index] { get; }
		IEnumerator<IVelocityTypeEntry> GetEnumerator();
	}
}