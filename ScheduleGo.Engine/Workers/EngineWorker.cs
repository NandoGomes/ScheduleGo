using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ScheduleGo.Engine.SwarmAlgorithms.PSO;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Entities;

namespace ScheduleGo.Engine.Workers
{
	public class EngineWorker : BackgroundService
	{
		private readonly ILogger<EngineWorker> _logger;

		public EngineWorker(ILogger<EngineWorker> logger) => _logger = logger;

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			// while (!stoppingToken.IsCancellationRequested)
			// {
			Swarm<PositionType, VelocityType> swarm = new Swarm<PositionType, VelocityType>(1000, 0.729, 1.49445);

			swarm.Build(100, 3, 1.49445)
				.OptimalFitnessRange(-2.999999999999999, 3.000000000000001);

			swarm.Process();
			// }
		}
	}
}
