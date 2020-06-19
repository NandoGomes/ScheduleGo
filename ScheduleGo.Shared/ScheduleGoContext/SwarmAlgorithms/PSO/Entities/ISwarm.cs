using System;

namespace ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Entities
{
	public interface ISwarm
	{
		int Lifetime { get; }
		double Weight { get; }
		double GlobalWeight { get; }
		int Iterations { get; }
		DateTime ProcessStart { get; }
		TimeSpan ProcessingDuration { get; }
	}
}