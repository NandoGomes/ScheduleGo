using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScheduleGo.Engine.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Capacity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                });

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
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    StudentsCount = table.Column<int>(nullable: false),
                    WeeklyWorkload = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
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
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimePeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassroomClassroomType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Classroom = table.Column<Guid>(nullable: false),
                    ClassroomType = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomClassroomType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassroomClassroomType_Classrooms_Classroom",
                        column: x => x.Classroom,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassroomClassroomType_ClassroomTypes_ClassroomType",
                        column: x => x.ClassroomType,
                        principalTable: "ClassroomTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseClassroomType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Course = table.Column<Guid>(nullable: false),
                    ClassroomType = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseClassroomType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseClassroomType_Courses_Course",
                        column: x => x.Course,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseClassroomType_ClassroomTypes_ClassroomType",
                        column: x => x.ClassroomType,
                        principalTable: "ClassroomTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClassroomTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Classroom = table.Column<Guid>(nullable: false),
                    Tag = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassroomTag_Classrooms_Classroom",
                        column: x => x.Classroom,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassroomTag_Tags_Tag",
                        column: x => x.Tag,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Course = table.Column<Guid>(nullable: false),
                    Tag = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTag_Courses_Course",
                        column: x => x.Course,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseTag_Tags_Tag",
                        column: x => x.Tag,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeacherQualifiedCourse",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Teacher = table.Column<Guid>(nullable: false),
                    Course = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherQualifiedCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherQualifiedCourse_Teachers_Teacher",
                        column: x => x.Teacher,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherQualifiedCourse_Teachers_Teacher1",
                        column: x => x.Teacher,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherQualifiedCourse_Courses_Course",
                        column: x => x.Course,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClassroomTimePeriod",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Classroom = table.Column<Guid>(nullable: false),
                    TimePeriod = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomTimePeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassroomTimePeriod_Classrooms_Classroom",
                        column: x => x.Classroom,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassroomTimePeriod_TimePeriods_TimePeriod",
                        column: x => x.TimePeriod,
                        principalTable: "TimePeriods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseTimePeriod",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Course = table.Column<Guid>(nullable: false),
                    TimePeriod = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTimePeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTimePeriod_Courses_Course",
                        column: x => x.Course,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseTimePeriod_TimePeriods_TimePeriod",
                        column: x => x.TimePeriod,
                        principalTable: "TimePeriods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeacherPreferredPeriod",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Teacher = table.Column<Guid>(nullable: false),
                    TimePeriod = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherPreferredPeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherPreferredPeriod_Teachers_Teacher",
                        column: x => x.Teacher,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherPreferredPeriod_Teachers_Teacher1",
                        column: x => x.Teacher,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherPreferredPeriod_TimePeriods_TimePeriod",
                        column: x => x.TimePeriod,
                        principalTable: "TimePeriods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomClassroomType_Classroom",
                table: "ClassroomClassroomType",
                column: "Classroom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomClassroomType_ClassroomType",
                table: "ClassroomClassroomType",
                column: "ClassroomType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomTag_Classroom",
                table: "ClassroomTag",
                column: "Classroom");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomTag_Tag",
                table: "ClassroomTag",
                column: "Tag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomTimePeriod_Classroom",
                table: "ClassroomTimePeriod",
                column: "Classroom");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomTimePeriod_TimePeriod",
                table: "ClassroomTimePeriod",
                column: "TimePeriod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseClassroomType_Course",
                table: "CourseClassroomType",
                column: "Course",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseClassroomType_ClassroomType",
                table: "CourseClassroomType",
                column: "ClassroomType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseTag_Course",
                table: "CourseTag",
                column: "Course");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTag_Tag",
                table: "CourseTag",
                column: "Tag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseTimePeriod_Course",
                table: "CourseTimePeriod",
                column: "Course");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTimePeriod_TimePeriod",
                table: "CourseTimePeriod",
                column: "TimePeriod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredPeriod_Teacher",
                table: "TeacherPreferredPeriod",
                column: "Teacher");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredPeriod_Teacher1",
                table: "TeacherPreferredPeriod",
                column: "Teacher");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredPeriod_TimePeriod",
                table: "TeacherPreferredPeriod",
                column: "TimePeriod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherQualifiedCourse_Teacher",
                table: "TeacherQualifiedCourse",
                column: "Teacher");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherQualifiedCourse_Teacher1",
                table: "TeacherQualifiedCourse",
                column: "Teacher");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherQualifiedCourse_Course",
                table: "TeacherQualifiedCourse",
                column: "Course",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassroomClassroomType");

            migrationBuilder.DropTable(
                name: "ClassroomTag");

            migrationBuilder.DropTable(
                name: "ClassroomTimePeriod");

            migrationBuilder.DropTable(
                name: "CourseClassroomType");

            migrationBuilder.DropTable(
                name: "CourseTag");

            migrationBuilder.DropTable(
                name: "CourseTimePeriod");

            migrationBuilder.DropTable(
                name: "TeacherPreferredPeriod");

            migrationBuilder.DropTable(
                name: "TeacherQualifiedCourse");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "ClassroomTypes");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "TimePeriods");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
