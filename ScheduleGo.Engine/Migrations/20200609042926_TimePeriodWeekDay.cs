using Microsoft.EntityFrameworkCore.Migrations;

namespace ScheduleGo.Engine.Migrations
{
    public partial class TimePeriodWeekDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeekDay",
                table: "TimePeriods",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeekDay",
                table: "TimePeriods");
        }
    }
}
