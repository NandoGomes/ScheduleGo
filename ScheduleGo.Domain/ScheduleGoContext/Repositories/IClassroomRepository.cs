using System;
using System.Collections.Generic;
using ScheduleGo.Domain.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Repositories
{
	public interface IClassroomRepository : IRepository<Classroom>
	{
		Dictionary<Guid, double> GetAverageSizePerType();
	}
}