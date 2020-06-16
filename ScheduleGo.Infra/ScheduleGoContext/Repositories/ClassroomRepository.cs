using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using ScheduleGo.Domain.ScheduleGoContext.Entities;
using ScheduleGo.Domain.ScheduleGoContext.Repositories;
using ScheduleGo.Infra.ScheduleGoContext.DataContexts;

namespace ScheduleGo.Infra.ScheduleGoContext.Repositories
{
	public class ClassroomRepository : Repository<Classroom>, IClassroomRepository
	{
		public ClassroomRepository(ScheduleGoDataContext context, ILogger<Repository<Classroom>> logger) : base(context, logger) { }

		public Dictionary<Guid, double> GetAverageSizePerType() => context.Classrooms.GroupBy(classroom => classroom.ClassroomTypeId).Select(group => new KeyValuePair<Guid, double>(group.Key, group.Average(classroom => classroom.Capacity))).ToDictionary(group => group.Key, group => group.Value);
	}
}