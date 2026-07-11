using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS.Infrastructre.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleTaskAndTimeTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "TimeTrackings");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "TimeTrackings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TimeTrackings");

            migrationBuilder.RenameColumn(
                name: "TotalPausedSeconds",
                table: "TimeTrackings",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "TimeTrackings",
                newName: "StartedAt");

            migrationBuilder.RenameColumn(
                name: "PausedAt",
                table: "TimeTrackings",
                newName: "EndedAt");

            migrationBuilder.RenameColumn(
                name: "PlannedTime",
                table: "ScheduleTasks",
                newName: "StartTime");

            migrationBuilder.AddColumn<int>(
                name: "AccumulatedSeconds",
                table: "TimeTrackings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaused",
                table: "TimeTrackings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "ScheduleTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccumulatedSeconds",
                table: "TimeTrackings");

            migrationBuilder.DropColumn(
                name: "IsPaused",
                table: "TimeTrackings");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "ScheduleTasks");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TimeTrackings",
                newName: "TotalPausedSeconds");

            migrationBuilder.RenameColumn(
                name: "StartedAt",
                table: "TimeTrackings",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "EndedAt",
                table: "TimeTrackings",
                newName: "PausedAt");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "ScheduleTasks",
                newName: "PlannedTime");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "TimeTrackings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "TimeTrackings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "TimeTrackings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
