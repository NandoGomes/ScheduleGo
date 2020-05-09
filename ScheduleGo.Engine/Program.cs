using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScheduleGo.Engine.Workers;

namespace ScheduleGo.Engine
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((hostingContext, config) => config.AddJsonFile("appsettings.json")
																 .AddJsonFile("appsettings.Development.json")
																 .AddEnvironmentVariables())
				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<EngineWorker>();
				});
	}
}
