using System.Collections.Generic;

namespace ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts
{
	public interface IPositionType
	{
		int Length { get; }

		IPositionType Build(int dimentions, object[] arguments);
		IPositionType Clone();

		void Update(int index, IVelocityTypeEntry value);

		IPositionTypeEntry this[int index] { get; }
		IEnumerator<IPositionTypeEntry> GetEnumerator();

		double CalculateFitness();
	}
}