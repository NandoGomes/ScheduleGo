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
                    End = table.Column<TimeSpan>(nullable: false),
                    WeekDay = table.Column<int>(nullable: false)
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
                    TeacherId = table.Column<Guid>(nullable: false),
                    TimePeriodId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherAvailablePeriod", x => new { x.TeacherId, x.TimePeriodId });
                    table.ForeignKey(
                        name: "FK_TeacherAvailablePeriod_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherAvailablePeriod_TimePeriods_TimePeriodId",
                        column: x => x.TimePeriodId,
                        principalTable: "TimePeriods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeacherPreferredPeriod",
                columns: table => new
                {
                    TeacherId = table.Column<Guid>(nullable: false),
                    TimePeriodId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherPreferredPeriod", x => new { x.TeacherId, x.TimePeriodId });
                    table.ForeignKey(
                        name: "FK_TeacherPreferredPeriod_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherPreferredPeriod_TimePeriods_TimePeriodId",
                        column: x => x.TimePeriodId,
                        principalTable: "TimePeriods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClassroomTag",
                columns: table => new
                {
                    ClassroomId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomTag", x => new { x.ClassroomId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ClassroomTag_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassroomTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClassroomTimePeriod",
                columns: table => new
                {
                    ClassroomId = table.Column<Guid>(nullable: false),
                    TimePeriodId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomTimePeriod", x => new { x.ClassroomId, x.TimePeriodId });
                    table.ForeignKey(
                        name: "FK_ClassroomTimePeriod_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassroomTimePeriod_TimePeriods_TimePeriodId",
                        column: x => x.TimePeriodId,
                        principalTable: "TimePeriods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseTag",
                columns: table => new
                {
                    CourseId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTag", x => new { x.CourseId, x.TagId });
                    table.ForeignKey(
                        name: "FK_CourseTag_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseTimePeriod",
                columns: table => new
                {
                    CourseId = table.Column<Guid>(nullable: false),
                    TimePeriodId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTimePeriod", x => new { x.CourseId, x.TimePeriodId });
                    table.ForeignKey(
                        name: "FK_CourseTimePeriod_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTimePeriod_TimePeriods_TimePeriodId",
                        column: x => x.TimePeriodId,
                        principalTable: "TimePeriods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeacherPreferredCourse",
                columns: table => new
                {
                    TeacherId = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherPreferredCourse", x => new { x.TeacherId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_TeacherPreferredCourse_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherPreferredCourse_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeacherQualifiedCourse",
                columns: table => new
                {
                    TeacherId = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherQualifiedCourse", x => new { x.TeacherId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_TeacherQualifiedCourse_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherQualifiedCourse_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_ClassroomTypeId",
                table: "Classrooms",
                column: "ClassroomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomTag_TagId",
                table: "ClassroomTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomTimePeriod_TimePeriodId",
                table: "ClassroomTimePeriod",
                column: "TimePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_NeededClassroomTypeId",
                table: "Courses",
                column: "NeededClassroomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTag_TagId",
                table: "CourseTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTimePeriod_TimePeriodId",
                table: "CourseTimePeriod",
                column: "TimePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAvailablePeriod_TimePeriodId",
                table: "TeacherAvailablePeriod",
                column: "TimePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredCourse_CourseId",
                table: "TeacherPreferredCourse",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredPeriod_TimePeriodId",
                table: "TeacherPreferredPeriod",
                column: "TimePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherQualifiedCourse_CourseId",
                table: "TeacherQualifiedCourse",
                column: "CourseId");
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
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "ClassroomTypes");
        }
    }
}
