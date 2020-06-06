using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScheduleGo.Domain.ScheduleGoContext.Repositories;
using ScheduleGo.Engine.Workers;
using ScheduleGo.Infra.ScheduleGoContext.DataContexts;
using ScheduleGo.Infra.ScheduleGoContext.Repositories;

namespace ScheduleGo.Engine
{
	public class Program
	{
		public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((hostingContext, config) => config.AddJsonFile("appsettings.json")
																 .AddJsonFile("appsettings.Development.json")
																 .AddEnvironmentVariables())
				.ConfigureServices((hostContext, services) =>
				{
					DbContextOptions<ScheduleGoDataContext> dbContextOptions = new DbContextOptionsBuilder<ScheduleGoDataContext>().UseLazyLoadingProxies().UseSqlServer(hostContext.Configuration["ConnectionString"], builder => builder.MigrationsAssembly(typeof(Program).Assembly.FullName)).Options;
					services.AddSingleton<ScheduleGoDataContext>(factory => new ScheduleGoDataContext(dbContextOptions));

					services.AddSingleton<ITeacherRepository, TeacherRepository>();
					services.AddSingleton<ICourseRepository, CourseRepository>();
					services.AddSingleton<ITimePeriodRepository, TimePeriodRepository>();
					services.AddSingleton<IClassroomRepository, ClassroomRepository>();

					services.AddHostedService<PSOWorker>();
				});
	}
}
