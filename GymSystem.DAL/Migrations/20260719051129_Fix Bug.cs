using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixBug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "SessionEndDateCheck",
                table: "Session");

            migrationBuilder.DropCheckConstraint(
                name: "SessionCapacityCheck",
                table: "Session");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Session",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "Session",
                newName: "EndDate");

            migrationBuilder.AddCheckConstraint(
                name: "SessionEndDateCheck",
                table: "Session",
                sql: "[EndDate] > [StartDate]");

            migrationBuilder.AddCheckConstraint(
                name: "SessionCapacityCheck",
                table: "Session",
                sql: "[Capacity] > 0");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "SessionEndDateCheck",
                table: "Session");

            migrationBuilder.DropCheckConstraint(
                name: "SessionCapacityCheck",
                table: "Session");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Session",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Session",
                newName: "EndTime");

            migrationBuilder.AddCheckConstraint(
                name: "SessionEndDateCheck",
                table: "Session",
                sql: "[EndTime] > [StartTime]");

            migrationBuilder.AddCheckConstraint(
                name: "SessionCapacityCheck",
                table: "Session",
                sql: "[Capacity] > 0");
        }
    }
}
