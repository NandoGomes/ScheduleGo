using ScheduleGo.Shared.ScheduleGoContext.Contracts;

namespace ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts
{
	public interface IVelocityTypeEntry : ICalculatable<IVelocityTypeEntry>
	{
		void Initialize();
	}
}