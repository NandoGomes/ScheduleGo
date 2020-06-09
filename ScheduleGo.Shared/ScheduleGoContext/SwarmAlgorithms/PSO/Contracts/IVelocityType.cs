using System.Collections.Generic;

namespace ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts
{
	public interface IVelocityType
	{
		int Length { get; }

		IVelocityType Build(int dimentions, object[] arguments);
		IVelocityType Clone();

		void Update(double weight,
					double particleWeight,
					double swarmWeight,
					IPositionType position,
					IPositionType bestPosition,
					IPositionType swarmBestPosition);

		IVelocityTypeEntry this[int index] { get; }
		IEnumerator<IVelocityTypeEntry> GetEnumerator();
	}
}