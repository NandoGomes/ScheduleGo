using Microsoft.Extensions.Logging;
using ScheduleGo.Domain.ScheduleGoContext.Entities;
using ScheduleGo.Domain.ScheduleGoContext.Repositories;
using ScheduleGo.Infra.ScheduleGoContext.DataContexts;

namespace ScheduleGo.Infra.ScheduleGoContext.Repositories
{
	public class ClassroomRepository : Repository<Classroom>, IClassroomRepository
	{
		public ClassroomRepository(ScheduleGoDataContext context, ILogger<Repository<Classroom>> logger) : base(context, logger) { }
	}
}