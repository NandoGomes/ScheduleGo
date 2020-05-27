using Microsoft.Extensions.Logging;
using ScheduleGo.Domain.ScheduleGoContext.Entities;
using ScheduleGo.Domain.ScheduleGoContext.Repositories;
using ScheduleGo.Infra.ScheduleGoContext.DataContexts;

namespace ScheduleGo.Infra.ScheduleGoContext.Repositories
{
	public class TeacherRepository : Repository<Teacher>, ITeacherRepository
	{
		public TeacherRepository(ScheduleGoDataContext context, ILogger<Repository<Teacher>> logger) : base(context, logger) { }
	}
}