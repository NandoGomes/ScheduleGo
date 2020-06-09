using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ScheduleGo.Domain.ScheduleGoContext.Entities;
using ScheduleGo.Domain.ScheduleGoContext.Repositories;
using ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Entities;

namespace ScheduleGo.Engine.Workers
{
	public class PSOWorker : BackgroundService
	{
		private readonly ITeacherRepository _teacherRepository;
		private readonly ICourseRepository _courseRepository;
		private readonly ITimePeriodRepository _timePeriodRepository;
		private readonly IClassroomRepository _classroomRepository;
		private readonly ILogger<PSOWorker> _logger;

		public PSOWorker(ITeacherRepository teacherRepository,
							ICourseRepository courseRepository,
							ITimePeriodRepository timePeriodRepository,
							IClassroomRepository classroomRepository,
							ILogger<PSOWorker> logger)
		{
			_teacherRepository = teacherRepository;
			_courseRepository = courseRepository;
			_timePeriodRepository = timePeriodRepository;
			_classroomRepository = classroomRepository;

			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				Swarm<PositionType, VelocityType> swarm = new Swarm<PositionType, VelocityType>(1000, 0.729, 1.49445);

				List<Teacher> teachers = _teacherRepository.GetAll().ToList();
				List<Course> courses = _courseRepository.GetAll().ToList();
				List<TimePeriod> timePeriods = _timePeriodRepository.GetAll().ToList();
				List<Classroom> classrooms = _classroomRepository.GetAll().ToList();

				double[] velocityMinValues = new double[] { -1, -1 };
				double[] velocityMaxValues = new double[] { courses.Count - 1, classrooms.Count - 1 };

				swarm.SetPositionArguments(teachers,
							   courses,
							   timePeriods,
							   classrooms,
							   3);

				swarm.SetVelocityArguments(velocityMinValues,
							   velocityMaxValues);

				swarm.Build(100,
							teachers.Count * timePeriods.Count,
							1.49445);

				swarm.Process();
			}
		}
	}
}