using ScheduleGo.Shared.ScheduleGoContext.Contracts;

namespace ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts
{
	public interface IPositionTypeEntry : ICalculatable<IPositionTypeEntry>
	{
		void Initialize();
	}
}