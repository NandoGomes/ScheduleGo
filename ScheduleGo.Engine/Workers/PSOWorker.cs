using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GemBox.Spreadsheet;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ScheduleGo.Domain.ScheduleGoContext.Entities;
using ScheduleGo.Domain.ScheduleGoContext.Enums;
using ScheduleGo.Domain.ScheduleGoContext.Repositories;
using ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO;
using ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Entities;

namespace ScheduleGo.Engine.Workers
{
	public class PSOWorker : BackgroundService
	{
		private readonly string _executionComment;

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
			_executionComment = "Smaller database";

			_teacherRepository = teacherRepository;
			_courseRepository = courseRepository;
			_timePeriodRepository = timePeriodRepository;
			_classroomRepository = classroomRepository;

			_logger = logger;
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			List<Teacher> teachers = _teacherRepository.GetAll().ToList();
			List<Course> courses = _courseRepository.GetAll().ToList();
			List<TimePeriod> timePeriods = _timePeriodRepository.GetAll().ToList();
			List<Classroom> classrooms = _classroomRepository.GetAll().ToList();

			Dictionary<Guid, double> classroomTypeAverageSize = _classroomRepository.GetAverageSizePerType();

			List<Course> treatedCourses = new List<Course>();

			foreach (Course course in courses)
			{
				if ((int)(course.StudentsCount / classroomTypeAverageSize[course.NeededClassroomTypeId]) > 1)
					treatedCourses.AddRange(course.Split((int)(course.StudentsCount / classroomTypeAverageSize[course.NeededClassroomTypeId])));

				else
					treatedCourses.Add(course);
			}

			courses = treatedCourses;

			int swarmSize = 5;
			int lifetime = 10;

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					int maxCoursesPerTeacher = 4;

					double weight = 0.729;
					double particleWeight = 1.49445;
					double globalWeight = 1.49445;

					int dimentions = teachers.Select(teacher => teacher.AvailablePeriods.Count()).Sum();

					double[] velocityMinValues = new double[] { 0, 0 };
					double[] velocityMaxValues = new double[] { courses.Count, classrooms.Count };

					Swarm<PositionType, VelocityType> swarm = new Swarm<PositionType, VelocityType>(lifetime, weight, globalWeight);

					swarm.SetPositionArguments(teachers,
								   courses,
								   timePeriods,
								   classrooms,
								   maxCoursesPerTeacher);

					swarm.SetVelocityArguments(velocityMinValues,
								   velocityMaxValues);

					swarm.Build(swarmSize,
								dimentions,
								particleWeight);

					swarm.Process();
					swarm.BestPosition.UpdateFitness();
					swarm.UpdateBestPosition(swarm.BestPosition);

					List<PositionTypeEntry> finalSchedule = swarm.BestPosition.Position.GetFinalSchedule().ToList();

					_validations(finalSchedule,
					 maxCoursesPerTeacher,
					 teachers,
					 courses,
					 classrooms,
					 timePeriods);

					_createSpreadsheet(swarm,
						   finalSchedule,
						   teachers,
						   courses,
						   classrooms,
						   timePeriods);

					swarmSize *= 10;
					lifetime *= 10;
				}
				catch { }
			}

			return null;
		}

		private void _validations(List<PositionTypeEntry> finalSchedule,
							int maxCoursesPerTeacher,
							List<Teacher> teachers,
							List<Course> courses,
							List<Classroom> classrooms,
							List<TimePeriod> timePeriods)
		{
			List<KeyValuePair<PositionTypeEntry, double>> orderedResult = finalSchedule.Select(entry => new KeyValuePair<PositionTypeEntry, double>(entry, entry.ToDouble())).OrderBy(pair => pair.Value).ToList();

			Dictionary<PositionTypeEntry, double> resultWithUnqualifiedTeachers = finalSchedule.Where(entry => !entry.Teacher.IsQualified(entry.Course)).Select(entry => new KeyValuePair<PositionTypeEntry, double>(entry, entry.ToDouble())).OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
			if (resultWithUnqualifiedTeachers.Any())
				throw new Exception("Fuck");

			Dictionary<PositionTypeEntry, double> resultsWithoutClassrooms = finalSchedule.Where(entry => entry.Classroom == null).Select(entry => new KeyValuePair<PositionTypeEntry, double>(entry, entry.ToDouble())).OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
			if (resultsWithoutClassrooms.Any())
				throw new Exception("Fuck");

			Dictionary<PositionTypeEntry, double> resultsWithInvalidClassrooms = finalSchedule.Where(entry => entry.Classroom?.ClassroomTypeId != entry.Course?.NeededClassroomTypeId).Select(entry => new KeyValuePair<PositionTypeEntry, double>(entry, entry.ToDouble())).OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
			if (resultsWithInvalidClassrooms.Any())
				throw new Exception("Fuck");

			Dictionary<string, string[]> coursesWithMultipleTeachers = finalSchedule.GroupBy(entry => entry.Course).Where(group => group.Select(entry => entry.Teacher).Distinct().Count() > 1).ToDictionary(group => group.Key.Name, group => group.Select(entry => entry.Teacher.Name).Distinct().ToArray());
			if (coursesWithMultipleTeachers.Any())
				throw new Exception("Fuck");

			List<string> coursesWithoutTeachers = courses.Except(finalSchedule.Select(entry => entry.Course).Distinct()).OrderBy(course => course.Name).Select(course => course.Name).ToList();
			if (coursesWithoutTeachers.Any())
				throw new Exception("Fuck");

			Dictionary<Teacher, List<Course>> teachersWithMoreCoursesThanAllowed = finalSchedule.GroupBy(entry => entry.Teacher).Select(group => new KeyValuePair<Teacher, List<Course>>(group.Key, group.Select(entry => entry.Course).Distinct().ToList())).Where(pair => pair.Value.Count > maxCoursesPerTeacher).ToDictionary(pair => pair.Key, pair => pair.Value);
			if (teachersWithMoreCoursesThanAllowed.Any())
				throw new Exception("Fuck");

			List<Teacher> teachersWithoutCourses = teachers.Except(finalSchedule.Select(entry => entry.Teacher).Distinct()).ToList();
			if (teachersWithoutCourses.Any())
				throw new Exception("Fuck");
		}

		private void _createSpreadsheet(Swarm<PositionType, VelocityType> swarm,
								  List<PositionTypeEntry> finalSchedule,
								  List<Teacher> teachers,
								  List<Course> courses,
								  List<Classroom> classrooms,
								  List<TimePeriod> timePeriods)
		{
			string folderPath = $"./Schedules/{DateTime.Today.ToString("yyyyMMdd")}";
			Directory.CreateDirectory(folderPath);

			var teachersSchedules = finalSchedule.GroupBy(entry => entry.Teacher.Name)
								  .ToDictionary(entry => entry.Key,
											  entry => entry.GroupBy(schedule => schedule.WeekDay)
												.OrderBy(schedule => schedule.Key)
												   .ToDictionary(schedule => schedule.Key,
															schedule => schedule.OrderBy(dailySchedule => dailySchedule.TimePeriod.Start)
																.GroupBy(dailySchedule => dailySchedule.TimePeriod.Description)
																   .ToDictionary(dailySchedule => dailySchedule.Key, dailySchedule => dailySchedule.ToList())));

			SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

			ExcelFile workbook = new ExcelFile();
			ExcelWorksheet worksheet = workbook.Worksheets.Add("Schedules");

			CellStyle headersStyle = new CellStyle
			{
				HorizontalAlignment = HorizontalAlignmentStyle.Center,
				VerticalAlignment = VerticalAlignmentStyle.Center
			};

			CellStyle scheduleEntryStyle = new CellStyle
			{
				HorizontalAlignment = HorizontalAlignmentStyle.Left,
				VerticalAlignment = VerticalAlignmentStyle.Center
			};

			headersStyle.FillPattern.SetSolid(SpreadsheetColor.FromName(ColorName.Black));
			headersStyle.Font.Color = SpreadsheetColor.FromName(ColorName.White);

			worksheet.Cells[0, 1].Value = "Sunday";
			worksheet.Cells[0, 2].Value = "Monday";
			worksheet.Cells[0, 3].Value = "Tuesday";
			worksheet.Cells[0, 4].Value = "Wednesday";
			worksheet.Cells[0, 5].Value = "Thursday";
			worksheet.Cells[0, 6].Value = "Friday";
			worksheet.Cells[0, 7].Value = "Saturday";

			worksheet.Cells[1, 0].Value = "Morning";
			worksheet.Cells[2, 0].Value = "Afternoon";
			worksheet.Cells[3, 0].Value = "Night";

			worksheet.Cells[5, 0].Value = "Course";
			worksheet.Cells[5, 1].Value = "Workload";

			worksheet.Cells[5, 3].Value = "Teacher";
			worksheet.Cells[5, 4].Value = "Course";

			worksheet.Cells[5, 6].Value = "Fitness";
			worksheet.Cells[6, 6].Value = swarm.BestPosition.Fitness.ToString();

			worksheet.Cells[8, 6].Value = "Comment";
			worksheet.Cells[9, 6].Value = _executionComment;

			worksheet.Cells[11, 6].Value = "Processing Duration";
			worksheet.Cells[12, 6].Value = swarm.ProcessingDuration.ToString();

			worksheet.Cells[14, 6].Value = "Weight";
			worksheet.Cells[14, 7].Value = "Particle's Weight";
			worksheet.Cells[14, 8].Value = "Global Weight";

			worksheet.Cells[15, 6].Value = swarm.Weight.ToString();
			worksheet.Cells[15, 7].Value = swarm.First().Weight.ToString();
			worksheet.Cells[15, 8].Value = swarm.GlobalWeight.ToString();

			worksheet.Cells[17, 6].Value = "Iteractions";
			worksheet.Cells[17, 7].Value = "Lifetime";

			worksheet.Cells[18, 6].Value = swarm.Iterations;
			worksheet.Cells[18, 7].Value = swarm.Lifetime;

			worksheet.Cells[20, 6].Value = "Number of particles";
			worksheet.Cells[21, 6].Value = swarm.Count;

			worksheet.Cells[0, 1].Style = headersStyle;
			worksheet.Cells[0, 2].Style = headersStyle;
			worksheet.Cells[0, 3].Style = headersStyle;
			worksheet.Cells[0, 4].Style = headersStyle;
			worksheet.Cells[0, 5].Style = headersStyle;
			worksheet.Cells[0, 6].Style = headersStyle;
			worksheet.Cells[0, 7].Style = headersStyle;

			worksheet.Cells[1, 0].Style = headersStyle;
			worksheet.Cells[2, 0].Style = headersStyle;
			worksheet.Cells[3, 0].Style = headersStyle;

			worksheet.Cells[5, 0].Style = headersStyle;
			worksheet.Cells[5, 1].Style = headersStyle;

			worksheet.Cells[5, 3].Style = headersStyle;
			worksheet.Cells[5, 4].Style = headersStyle;

			worksheet.Cells[5, 6].Style = headersStyle;

			worksheet.Cells[8, 6].Style = headersStyle;

			worksheet.Cells[11, 6].Style = headersStyle;

			worksheet.Cells[14, 6].Style = headersStyle;
			worksheet.Cells[14, 7].Style = headersStyle;
			worksheet.Cells[14, 8].Style = headersStyle;

			worksheet.Cells[17, 6].Style = headersStyle;
			worksheet.Cells[17, 7].Style = headersStyle;

			worksheet.Cells[20, 6].Style = headersStyle;

			foreach (var teacherSchedule in teachersSchedules)
				foreach (KeyValuePair<EWeekDay, Dictionary<string, List<PositionTypeEntry>>> day in teacherSchedule.Value)
				{
					int collumn = ((int)day.Key) + 1;
					int row = 0;

					foreach (KeyValuePair<string, List<PositionTypeEntry>> scheduleEntry in day.Value)
					{
						if (scheduleEntry.Key == "Morning")
							row = 1;

						else if (scheduleEntry.Key == "Afternoon")
							row = 2;

						else
							row = 3;

						worksheet.Cells[row, collumn].Style = scheduleEntryStyle;
						worksheet.Cells[row, collumn].Value += $"{teacherSchedule.Key}: {scheduleEntry.Value.First().Course?.Name ?? "ERRO"} - {scheduleEntry.Value.First().Classroom?.Description ?? "ERRO"}\n";
					}

					row = 6;

					foreach (KeyValuePair<Course, TimeSpan> courseWorkload in swarm.BestPosition.Position.CoursesWorkload)
					{
						CellStyle courseWorkloadStyle = new CellStyle();

						if (courseWorkload.Value < courseWorkload.Key.WeeklyWorkload)
						{
							courseWorkloadStyle.FillPattern.SetSolid(SpreadsheetColor.FromName(ColorName.Red));
							courseWorkloadStyle.Font.Color = SpreadsheetColor.FromName(ColorName.White);
						}

						else if (courseWorkload.Value == courseWorkload.Key.WeeklyWorkload)
						{
							courseWorkloadStyle.FillPattern.SetSolid(SpreadsheetColor.FromName(ColorName.Green));
							courseWorkloadStyle.Font.Color = SpreadsheetColor.FromName(ColorName.Black);
						}

						else
						{
							courseWorkloadStyle.FillPattern.SetSolid(SpreadsheetColor.FromName(ColorName.Yellow));
							courseWorkloadStyle.Font.Color = SpreadsheetColor.FromName(ColorName.Black);
						}

						worksheet.Cells[row, 0].Value = courseWorkload.Key.Name;
						worksheet.Cells[row, 1].Value = courseWorkload.Value.ToString();
						worksheet.Cells[row, 1].Style = courseWorkloadStyle;

						row++;
					}

					row = 6;

					foreach (KeyValuePair<Teacher, HashSet<Course>> teacherCourses in swarm.BestPosition.Position.TeacherAssignedCourses)
					{
						worksheet.Cells[row, 3].Value = teacherCourses.Key.Name;

						foreach (Course course in teacherCourses.Value)
						{
							CellStyle teacherCourseStyle = new CellStyle();

							if (!teacherCourses.Key.IsQualified(course))
							{
								teacherCourseStyle.FillPattern.SetSolid(SpreadsheetColor.FromName(ColorName.Red));
								teacherCourseStyle.Font.Color = SpreadsheetColor.FromName(ColorName.White);
							}

							else if (teacherCourses.Key.Prefers(course))
							{
								teacherCourseStyle.FillPattern.SetSolid(SpreadsheetColor.FromName(ColorName.Green));
								teacherCourseStyle.Font.Color = SpreadsheetColor.FromName(ColorName.Black);
							}

							else
							{
								teacherCourseStyle.FillPattern.SetSolid(SpreadsheetColor.FromName(ColorName.Yellow));
								teacherCourseStyle.Font.Color = SpreadsheetColor.FromName(ColorName.Black);
							}

							worksheet.Cells[row, 4].Value = course.Name;
							worksheet.Cells[row, 4].Style = teacherCourseStyle;

							row++;
						}

						row++;
					}
				}

			workbook.Save($"{folderPath}/{DateTime.Now.ToString("HHmmss")}_{(_executionComment.Length >= 6 ? _executionComment.Substring(0, 10) : _executionComment)}.xlsx");
		}
	}
}