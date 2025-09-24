using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CavistaEventCelebration.Api.Migrations
{
    /// <inheritdoc />
    public partial class addapprovalforemployeeevent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedBy",
                table: "EmployeeEvents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateApproved",
                table: "EmployeeEvents",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "EmployeeEvents",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "EmployeeEvents");

            migrationBuilder.DropColumn(
                name: "DateApproved",
                table: "EmployeeEvents");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "EmployeeEvents");
        }
    }
}
