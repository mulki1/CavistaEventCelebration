using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CavistaEventCelebration.Api.Migrations
{
    public partial class addemployeeIdtoaspnetuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Add EmployeeId column as nullable first
            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            // 2. Create Employees for any users that don’t already have one
            migrationBuilder.Sql(@"
                INSERT INTO ""Employees"" (""Id"", ""EmailAddress"", ""FirstName"", ""LastName"", ""IsDeprecated"")
                SELECT gen_random_uuid(), u.""Email"", 'Unknown', 'Unknown', false
                FROM ""AspNetUsers"" u
                WHERE NOT EXISTS (
                    SELECT 1 FROM ""Employees"" e WHERE e.""EmailAddress"" = u.""Email""
                );
            ");

            // 3. Backfill EmployeeId for all users
            migrationBuilder.Sql(@"
                UPDATE ""AspNetUsers"" u
                SET ""EmployeeId"" = e.""Id""
                FROM ""Employees"" e
                WHERE u.""Email"" = e.""EmailAddress"";
            ");

            // 4. Make EmployeeId required (NOT NULL)
            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            // 5. Add unique index + FK constraint
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeeId",
                table: "AspNetUsers",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeId",
                table: "AspNetUsers",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployeeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AspNetUsers");
        }
    }
}
