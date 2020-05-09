using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ScheduleGo.Engine.Workers
{
	public class EngineWorker : BackgroundService
	{
		private readonly ILogger<EngineWorker> _logger;

		public EngineWorker(ILogger<EngineWorker> logger) => _logger = logger;

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				await Task.Delay(1000, stoppingToken);
			}
		}
	}
}
