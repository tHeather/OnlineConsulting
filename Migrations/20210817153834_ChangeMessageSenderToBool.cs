using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineConsulting.Migrations
{
    public partial class ChangeMessageSenderToBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sender",
                table: "ChatMessages");

            migrationBuilder.AddColumn<bool>(
                name: "IsFromClient",
                table: "ChatMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFromClient",
                table: "ChatMessages");

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
    }
}
