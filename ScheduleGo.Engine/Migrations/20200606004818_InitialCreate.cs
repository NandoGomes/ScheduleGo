using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScheduleGo.Engine.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassroomTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimePeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Start = table.Column<TimeSpan>(nullable: false),
                    End = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimePeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Capacity = table.Column<int>(nullable: false),
                    ClassroomTypeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classrooms_ClassroomTypes_ClassroomTypeId",
                        column: x => x.ClassroomTypeId,
                        principalTable: "ClassroomTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    StudentsCount = table.Column<int>(nullable: false),
                    NeededClassroomTypeId = table.Column<Guid>(nullable: false),
                    WeeklyWorkload = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_ClassroomTypes_NeededClassroomTypeId",
                        column: x => x.NeededClassroomTypeId,
                        principalTable: "ClassroomTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeacherAvailablePeriod",
                columns: table => new
                {
                    Teacher = table.Column<Guid>(nullable: false),
                    TimePeriod = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherAvailablePeriod", x => new { x.Teacher, x.TimePeriod });
                    table.ForeignKey(
                        name: "FK_TeacherAvailablePeriod_Teachers_Teacher",
                        column: x => x.Teacher,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherAvailablePeriod_TimePeriods_TimePeriod",
                        column: x => x.TimePeriod,
                        principalTable: "TimePeriods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeacherPreferredPeriod",
                columns: table => new
                {
                    Teacher = table.Column<Guid>(nullable: false),
                    TimePeriod = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherPreferredPeriod", x => new { x.Teacher, x.TimePeriod });
                    table.ForeignKey(
                        name: "FK_TeacherPreferredPeriod_Teachers_Teacher",
                        column: x => x.Teacher,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherPreferredPeriod_TimePeriods_TimePeriod",
                        column: x => x.TimePeriod,
                        principalTable: "TimePeriods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClassroomTag",
                columns: table => new
                {
                    Classroom = table.Column<Guid>(nullable: false),
                    Tag = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomTag", x => new { x.Classroom, x.Tag });
                    table.ForeignKey(
                        name: "FK_ClassroomTag_Classrooms_Classroom",
                        column: x => x.Classroom,
                        principalTable: "Classrooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClassroomTag_Tags_Tag",
                        column: x => x.Tag,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClassroomTimePeriod",
                columns: table => new
                {
                    Classroom = table.Column<Guid>(nullable: false),
                    TimePeriod = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomTimePeriod", x => new { x.Classroom, x.TimePeriod });
                    table.ForeignKey(
                        name: "FK_ClassroomTimePeriod_Classrooms_Classroom",
                        column: x => x.Classroom,
                        principalTable: "Classrooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClassroomTimePeriod_TimePeriods_TimePeriod",
                        column: x => x.TimePeriod,
                        principalTable: "TimePeriods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseTag",
                columns: table => new
                {
                    Course = table.Column<Guid>(nullable: false),
                    Tag = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTag", x => new { x.Course, x.Tag });
                    table.ForeignKey(
                        name: "FK_CourseTag_Courses_Course",
                        column: x => x.Course,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseTag_Tags_Tag",
                        column: x => x.Tag,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseTimePeriod",
                columns: table => new
                {
                    Course = table.Column<Guid>(nullable: false),
                    TimePeriod = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTimePeriod", x => new { x.Course, x.TimePeriod });
                    table.ForeignKey(
                        name: "FK_CourseTimePeriod_Courses_Course",
                        column: x => x.Course,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseTimePeriod_TimePeriods_TimePeriod",
                        column: x => x.TimePeriod,
                        principalTable: "TimePeriods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeacherPreferredCourse",
                columns: table => new
                {
                    Teacher = table.Column<Guid>(nullable: false),
                    Course = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherPreferredCourse", x => new { x.Teacher, x.Course });
                    table.ForeignKey(
                        name: "FK_TeacherPreferredCourse_Teachers_Teacher",
                        column: x => x.Teacher,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherPreferredCourse_Courses_Course",
                        column: x => x.Course,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeacherQualifiedCourse",
                columns: table => new
                {
                    Teacher = table.Column<Guid>(nullable: false),
                    Course = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherQualifiedCourse", x => new { x.Teacher, x.Course });
                    table.ForeignKey(
                        name: "FK_TeacherQualifiedCourse_Teachers_Teacher",
                        column: x => x.Teacher,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherQualifiedCourse_Courses_Course",
                        column: x => x.Course,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_ClassroomTypeId",
                table: "Classrooms",
                column: "ClassroomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomTag_Tag",
                table: "ClassroomTag",
                column: "Tag");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomTimePeriod_TimePeriod",
                table: "ClassroomTimePeriod",
                column: "TimePeriod");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_NeededClassroomTypeId",
                table: "Courses",
                column: "NeededClassroomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTag_Tag",
                table: "CourseTag",
                column: "Tag");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTimePeriod_TimePeriod",
                table: "CourseTimePeriod",
                column: "TimePeriod");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAvailablePeriod_TimePeriod",
                table: "TeacherAvailablePeriod",
                column: "TimePeriod");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredCourse_Course",
                table: "TeacherPreferredCourse",
                column: "Course");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredPeriod_TimePeriod",
                table: "TeacherPreferredPeriod",
                column: "TimePeriod");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherQualifiedCourse_Course",
                table: "TeacherQualifiedCourse",
                column: "Course");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassroomTag");

            migrationBuilder.DropTable(
                name: "ClassroomTimePeriod");

            migrationBuilder.DropTable(
                name: "CourseTag");

            migrationBuilder.DropTable(
                name: "CourseTimePeriod");

            migrationBuilder.DropTable(
                name: "TeacherAvailablePeriod");

            migrationBuilder.DropTable(
                name: "TeacherPreferredCourse");

            migrationBuilder.DropTable(
                name: "TeacherPreferredPeriod");

            migrationBuilder.DropTable(
                name: "TeacherQualifiedCourse");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "TimePeriods");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "ClassroomTypes");
        }
    }
}
