using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineConsulting.Migrations
{
    public partial class AddSenderToChatMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51802d91-7fa7-436c-9873-a201c8a35bfb",
                column: "ConcurrencyStamp",
                value: "0cc57c03-e2b8-4d6d-9044-242e99d7b772");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c319ab1e-f914-4ebb-8ac9-d6da40d88419",
                column: "ConcurrencyStamp",
                value: "1c03132b-1382-4864-999a-20d3e0790fe0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1dbd6ec-4d0e-4f0a-bd9f-125cb168ff42",
                column: "ConcurrencyStamp",
                value: "ee406ccc-7aa0-4058-9866-ec66e14b5a38");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sender",
                table: "ChatMessages");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51802d91-7fa7-436c-9873-a201c8a35bfb",
                column: "ConcurrencyStamp",
                value: "12603279-97d3-4ef4-b48a-3bb0d4f6eb2b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c319ab1e-f914-4ebb-8ac9-d6da40d88419",
                column: "ConcurrencyStamp",
                value: "2e37d2cb-228c-417e-9dac-3b5edb734ddb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1dbd6ec-4d0e-4f0a-bd9f-125cb168ff42",
                column: "ConcurrencyStamp",
                value: "9f4bdb89-1893-4406-8898-0109756f5666");
        }
    }
}
