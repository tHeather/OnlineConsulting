using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineConsulting.Migrations
{
    public partial class removeEmployerSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_EmployerSettings_EmployerSettingId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "EmployerSettings");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployerSettingId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployerSettingId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51802d91-7fa7-436c-9873-a201c8a35bfb",
                column: "ConcurrencyStamp",
                value: "dd7b7098-340d-453f-a397-7fdfe3cf5fcb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c319ab1e-f914-4ebb-8ac9-d6da40d88419",
                column: "ConcurrencyStamp",
                value: "02b6a1d5-3c7a-43c9-b6ad-6cdd6ec5a975");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1dbd6ec-4d0e-4f0a-bd9f-125cb168ff42",
                column: "ConcurrencyStamp",
                value: "8fc7534a-5fef-44fa-8be2-3d12fd619e68");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployerSettingId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployerSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Domain = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: true),
                    SubscriptionEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployerSettings", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51802d91-7fa7-436c-9873-a201c8a35bfb",
                column: "ConcurrencyStamp",
                value: "2adfd19a-c79b-4bb2-b701-3f03b827a494");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c319ab1e-f914-4ebb-8ac9-d6da40d88419",
                column: "ConcurrencyStamp",
                value: "c06e8c36-6e5c-421f-9d83-bf149b3975f2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1dbd6ec-4d0e-4f0a-bd9f-125cb168ff42",
                column: "ConcurrencyStamp",
                value: "43d5146f-87f4-4a8e-bcb9-bcdd4970351e");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployerSettingId",
                table: "AspNetUsers",
                column: "EmployerSettingId",
                unique: true,
                filter: "[EmployerSettingId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_EmployerSettings_EmployerSettingId",
                table: "AspNetUsers",
                column: "EmployerSettingId",
                principalTable: "EmployerSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
