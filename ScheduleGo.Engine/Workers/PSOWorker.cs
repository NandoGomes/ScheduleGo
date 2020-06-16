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

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				string folderPath = $"./Schedules/{DateTime.Now.ToString("yyyyMMdd/HH_mm_ss")}";
				Directory.CreateDirectory(folderPath);

				Swarm<PositionType, VelocityType> swarm = new Swarm<PositionType, VelocityType>(100, 0.729, 1.49445);

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

				double[] velocityMinValues = new double[] { 0, 0 };
				double[] velocityMaxValues = new double[] { courses.Count, classrooms.Count };

				swarm.SetPositionArguments(teachers,
							   courses,
							   timePeriods,
							   classrooms,
							   4);

				swarm.SetVelocityArguments(velocityMinValues,
							   velocityMaxValues);

				swarm.Build(100,
							teachers.Select(teacher => teacher.AvailablePeriods.Count()).Sum(),
							1.49445);

				swarm.Process();

				var result = (swarm.BestPosition.Position as PositionType).GetFinalSchedule();

				var orderedResult = result.Select(entry => new KeyValuePair<PositionTypeEntry, double>(entry, entry.ToDouble())).OrderBy(pair => pair.Value).ToList();

				var resultWithUnqualifiedTeachers = result.Where(entry => !entry.Teacher.IsQualified(entry.Course)).Select(entry => new KeyValuePair<PositionTypeEntry, double>(entry, entry.ToDouble())).OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
				var resultsWithoutClassrooms = result.Where(entry => entry.Classroom == null).Select(entry => new KeyValuePair<PositionTypeEntry, double>(entry, entry.ToDouble())).OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
				var resultsWithInvalidClassrooms = result.Where(entry => entry.Classroom?.ClassroomTypeId != entry.Course?.NeededClassroomTypeId).Select(entry => new KeyValuePair<PositionTypeEntry, double>(entry, entry.ToDouble())).OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
				var CoursesWithMultipleTeachers = result.GroupBy(entry => entry.Course).Where(group => group.Select(entry => entry.Teacher).Distinct().Count() > 1).ToDictionary(group => group.Key.Name, group => group.Select(entry => entry.Teacher.Name).Distinct().ToArray());
				var coursesWithoutTeachers = courses.Except(result.Select(entry => entry.Course).Distinct()).OrderBy(course => course.Name).Select(course => course.Name).ToList();

				var sundays = result.Where(entry => entry.WeekDay == EWeekDay.Sunday).GroupBy(entry => entry.TimePeriod.Description).ToDictionary(entry => entry.Key, entry => entry.ToList());
				var mondays = result.Where(entry => entry.WeekDay == EWeekDay.Monday).GroupBy(entry => entry.TimePeriod.Description).ToDictionary(entry => entry.Key, entry => entry.ToList());
				var tuesdays = result.Where(entry => entry.WeekDay == EWeekDay.Tuesday).GroupBy(entry => entry.TimePeriod.Description).ToDictionary(entry => entry.Key, entry => entry.ToList());
				var wednesdays = result.Where(entry => entry.WeekDay == EWeekDay.Wednesday).GroupBy(entry => entry.TimePeriod.Description).ToDictionary(entry => entry.Key, entry => entry.ToList());
				var thursdays = result.Where(entry => entry.WeekDay == EWeekDay.Thursday).GroupBy(entry => entry.TimePeriod.Description).ToDictionary(entry => entry.Key, entry => entry.ToList());
				var fridays = result.Where(entry => entry.WeekDay == EWeekDay.Friday).GroupBy(entry => entry.TimePeriod.Description).ToDictionary(entry => entry.Key, entry => entry.ToList());
				var saturdays = result.Where(entry => entry.WeekDay == EWeekDay.Saturday).GroupBy(entry => entry.TimePeriod.Description).ToDictionary(entry => entry.Key, entry => entry.ToList());

				var coursesPerTeacher = result.GroupBy(entry => entry.Teacher).ToDictionary(group => group.Key, group => group.Select(entry => entry.Course).Distinct().ToList());

				var teachersSchedules = result.GroupBy(entry => entry.Teacher.Name)
								  .ToDictionary(entry => entry.Key,
								  			entry => entry.GroupBy(schedule => schedule.WeekDay)
												.OrderBy(schedule => schedule.Key)
						   						.ToDictionary(schedule => schedule.Key,
															schedule => schedule.OrderBy(dailySchedule => dailySchedule.TimePeriod.Start)
																.GroupBy(dailySchedule => dailySchedule.TimePeriod.Description)
			   													.ToDictionary(dailySchedule => dailySchedule.Key, dailySchedule => dailySchedule.ToList())));

				var finalFitness = swarm.BestPosition.Position.CalculateFitness();

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

				Dictionary<Course, TimeSpan> coursesWorkload = courses.OrderBy(course => course.Name).ToDictionary(course => course, course => new TimeSpan());

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
							coursesWorkload[scheduleEntry.Value.First().Course] += scheduleEntry.Value.First().TimePeriod.Duration;
						}

						row = 6;

						foreach (KeyValuePair<Course, TimeSpan> courseWorkload in coursesWorkload)
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

						foreach (KeyValuePair<Teacher, List<Course>> teacherCourses in coursesPerTeacher)
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

				workbook.Save($"{folderPath}/{finalFitness}.xlsx");
			}

			return null;
		}
	}
}