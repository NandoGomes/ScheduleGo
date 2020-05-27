using Microsoft.Extensions.Logging;
using ScheduleGo.Domain.ScheduleGoContext.Entities;
using ScheduleGo.Domain.ScheduleGoContext.Repositories;
using ScheduleGo.Infra.ScheduleGoContext.DataContexts;

namespace ScheduleGo.Infra.ScheduleGoContext.Repositories
{
	public class CourseRepository : Repository<Course>, ICourseRepository
	{
		public CourseRepository(ScheduleGoDataContext context, ILogger<Repository<Course>> logger) : base(context, logger) { }
	}
}