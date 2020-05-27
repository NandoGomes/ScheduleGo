using Microsoft.Extensions.Logging;
using ScheduleGo.Domain.ScheduleGoContext.Entities;
using ScheduleGo.Domain.ScheduleGoContext.Repositories;
using ScheduleGo.Infra.ScheduleGoContext.DataContexts;

namespace ScheduleGo.Infra.ScheduleGoContext.Repositories
{
	public class TimePeriodRepository : Repository<TimePeriod>, ITimePeriodRepository
	{
		public TimePeriodRepository(ScheduleGoDataContext context, ILogger<Repository<TimePeriod>> logger) : base(context, logger) { }
	}
}