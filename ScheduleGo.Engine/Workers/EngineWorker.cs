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
    public class EngineWorker : BackgroundService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ITimePeriodRepository _timePeriodRepository;
        private readonly IClassroomRepository _classroomRepository;
        private readonly ILogger<EngineWorker> _logger;

        public EngineWorker(ITeacherRepository teacherRepository,
                            ICourseRepository courseRepository,
                            ITimePeriodRepository timePeriodRepository,
                            IClassroomRepository classroomRepository,
                            ILogger<EngineWorker> logger)
        {
            _teacherRepository = teacherRepository;
            _courseRepository = courseRepository;
            _timePeriodRepository = timePeriodRepository;
            _classroomRepository = classroomRepository;

            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // while (!stoppingToken.IsCancellationRequested)
            // {
            Swarm<PositionType, VelocityType> swarm = new Swarm<PositionType, VelocityType>(1000, 0.729, 1.49445);

            IEnumerable<Teacher> teachers = _teacherRepository.GetAll();
            IEnumerable<Course> courses = _courseRepository.GetAll();
            IEnumerable<TimePeriod> timePeriods = _timePeriodRepository.GetAll();
            IEnumerable<Classroom> classrooms = _classroomRepository.GetAll();

            double[] velocityMinValues = new double[] { 0, 0 };
            double[] velocityMaxValues = new double[] { courses.Count(), classrooms.Count() };

            swarm.Build(100,
                        3,
                        1.49445,
                        velocityMinValues,
                        velocityMaxValues,
                        teachers,
                        courses,
                        timePeriods,
                        classrooms);

            swarm.Process();
            // }
        }
    }
}
