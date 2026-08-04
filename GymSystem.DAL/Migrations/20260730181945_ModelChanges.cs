using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Booking_BookingId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Member_MemberId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Session_SessionId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecord_Member_MemberId",
                table: "HealthRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberShip_Member_MemberId",
                table: "MemberShip");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberShip_Plans_PlanId",
                table: "MemberShip");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Category_CategoryId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Session_Category_CategoryId",
                table: "Session");

            migrationBuilder.DropForeignKey(
                name: "FK_Session_Trainer_TrainerId",
                table: "Session");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trainer",
                table: "Trainer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Session",
                table: "Session");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberShip",
                table: "MemberShip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Member",
                table: "Member");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthRecord",
                table: "HealthRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "StartDate1",
                table: "MemberShip");

            migrationBuilder.RenameTable(
                name: "Trainer",
                newName: "Trainers");

            migrationBuilder.RenameTable(
                name: "Session",
                newName: "Sessions");

            migrationBuilder.RenameTable(
                name: "MemberShip",
                newName: "Memberships");

            migrationBuilder.RenameTable(
                name: "Member",
                newName: "Members");

            migrationBuilder.RenameTable(
                name: "HealthRecord",
                newName: "HealthRecords");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "Bookings");

            migrationBuilder.RenameIndex(
                name: "IX_Trainer_PhoneNumber",
                table: "Trainers",
                newName: "IX_Trainers_PhoneNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Trainer_Email",
                table: "Trainers",
                newName: "IX_Trainers_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Session_TrainerId",
                table: "Sessions",
                newName: "IX_Sessions_TrainerId");

            migrationBuilder.RenameIndex(
                name: "IX_Session_CategoryId",
                table: "Sessions",
                newName: "IX_Sessions_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_MemberShip_PlanId",
                table: "Memberships",
                newName: "IX_Memberships_PlanId");

            migrationBuilder.RenameIndex(
                name: "IX_MemberShip_MemberId",
                table: "Memberships",
                newName: "IX_Memberships_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Member_PhoneNumber",
                table: "Members",
                newName: "IX_Members_PhoneNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Member_Email",
                table: "Members",
                newName: "IX_Members_Email");

            migrationBuilder.RenameIndex(
                name: "IX_HealthRecord_MemberId",
                table: "HealthRecords",
                newName: "IX_HealthRecords_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_SessionId",
                table: "Bookings",
                newName: "IX_Bookings_SessionId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_MemberId",
                table: "Bookings",
                newName: "IX_Bookings_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_BookingId",
                table: "Bookings",
                newName: "IX_Bookings_BookingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trainers",
                table: "Trainers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Memberships",
                table: "Memberships",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthRecords",
                table: "HealthRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Bookings_BookingId",
                table: "Bookings",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Members_MemberId",
                table: "Bookings",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Sessions_SessionId",
                table: "Bookings",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecords_Members_MemberId",
                table: "HealthRecords",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Members_MemberId",
                table: "Memberships",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Plans_PlanId",
                table: "Memberships",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Categories_CategoryId",
                table: "Plans",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Categories_CategoryId",
                table: "Sessions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Trainers_TrainerId",
                table: "Sessions",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Bookings_BookingId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Members_MemberId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Sessions_SessionId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecords_Members_MemberId",
                table: "HealthRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Members_MemberId",
                table: "Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Plans_PlanId",
                table: "Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Categories_CategoryId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Categories_CategoryId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Trainers_TrainerId",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trainers",
                table: "Trainers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Memberships",
                table: "Memberships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthRecords",
                table: "HealthRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "Trainers",
                newName: "Trainer");

            migrationBuilder.RenameTable(
                name: "Sessions",
                newName: "Session");

            migrationBuilder.RenameTable(
                name: "Memberships",
                newName: "MemberShip");

            migrationBuilder.RenameTable(
                name: "Members",
                newName: "Member");

            migrationBuilder.RenameTable(
                name: "HealthRecords",
                newName: "HealthRecord");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "Booking");

            migrationBuilder.RenameIndex(
                name: "IX_Trainers_PhoneNumber",
                table: "Trainer",
                newName: "IX_Trainer_PhoneNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Trainers_Email",
                table: "Trainer",
                newName: "IX_Trainer_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_TrainerId",
                table: "Session",
                newName: "IX_Session_TrainerId");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_CategoryId",
                table: "Session",
                newName: "IX_Session_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Memberships_PlanId",
                table: "MemberShip",
                newName: "IX_MemberShip_PlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Memberships_MemberId",
                table: "MemberShip",
                newName: "IX_MemberShip_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Members_PhoneNumber",
                table: "Member",
                newName: "IX_Member_PhoneNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Members_Email",
                table: "Member",
                newName: "IX_Member_Email");

            migrationBuilder.RenameIndex(
                name: "IX_HealthRecords_MemberId",
                table: "HealthRecord",
                newName: "IX_HealthRecord_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_SessionId",
                table: "Booking",
                newName: "IX_Booking_SessionId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_MemberId",
                table: "Booking",
                newName: "IX_Booking_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_BookingId",
                table: "Booking",
                newName: "IX_Booking_BookingId");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate1",
                table: "MemberShip",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trainer",
                table: "Trainer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Session",
                table: "Session",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberShip",
                table: "MemberShip",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Member",
                table: "Member",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthRecord",
                table: "HealthRecord",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Booking_BookingId",
                table: "Booking",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Member_MemberId",
                table: "Booking",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Session_SessionId",
                table: "Booking",
                column: "SessionId",
                principalTable: "Session",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecord_Member_MemberId",
                table: "HealthRecord",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShip_Member_MemberId",
                table: "MemberShip",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShip_Plans_PlanId",
                table: "MemberShip",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Category_CategoryId",
                table: "Plans",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Session_Category_CategoryId",
                table: "Session",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Session_Trainer_TrainerId",
                table: "Session",
                column: "TrainerId",
                principalTable: "Trainer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
