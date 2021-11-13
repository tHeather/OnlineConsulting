using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineConsulting.Migrations
{
    public partial class AddIsAccountLocketToUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccountLocked",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51802d91-7fa7-436c-9873-a201c8a35bfb",
                column: "ConcurrencyStamp",
                value: "2d513a9b-4ee8-4714-92aa-4046d0a55bea");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c319ab1e-f914-4ebb-8ac9-d6da40d88419",
                column: "ConcurrencyStamp",
                value: "48f13fe0-1c90-4b47-991b-6e07c502597b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1dbd6ec-4d0e-4f0a-bd9f-125cb168ff42",
                column: "ConcurrencyStamp",
                value: "96854a89-c603-4f47-91f0-938469ba0945");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7f10381c-7a7c-4e65-b468-a5b32c789720",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "8bb8dd10-356a-4aab-8281-95946d562068", "58e2a692-2efc-473a-8fce-f66f30af504f" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccountLocked",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51802d91-7fa7-436c-9873-a201c8a35bfb",
                column: "ConcurrencyStamp",
                value: "cca16aab-f5d4-43a8-8921-b752d73478d5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c319ab1e-f914-4ebb-8ac9-d6da40d88419",
                column: "ConcurrencyStamp",
                value: "f6f45176-7327-4889-ae17-4b198ae4f141");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1dbd6ec-4d0e-4f0a-bd9f-125cb168ff42",
                column: "ConcurrencyStamp",
                value: "45237011-4821-4815-bf27-db0eaa510984");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7f10381c-7a7c-4e65-b468-a5b32c789720",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a4618fa9-0704-4ace-ba36-d8c2fdd20e73", "709dc567-943c-4456-8f98-622336050e7c" });
        }
    }
}
