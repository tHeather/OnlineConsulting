using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineConsulting.Migrations
{
    public partial class ChangeUserEnityConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_EmployerSettings_EmployerSettingId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployerSettingId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0911eb8e-d149-46f2-b719-5717be975bd6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73c84071-a08e-4563-b7ad-92a713339820");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4e12306-e927-4c14-b214-31244fe7a609");

            migrationBuilder.AlterColumn<int>(
                name: "EmployerSettingId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "466c4c05-2a69-4d64-8a39-e8db4f6b5b04", "28a43ad5-46e1-41f4-b71b-a04224a4bc68", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "82fcd2ee-521a-4cbf-8c15-cae05a702688", "473d15df-47d7-41b2-b615-d8786a13d2a9", "Employer", "EMPLOYER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1e0bdedb-d6c8-49ad-bbf3-8d5ee2d52df6", "4f552f7d-636e-4451-a712-eca920cc47aa", "Consultant", "CONSULTANT" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_EmployerSettings_EmployerSettingId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployerSettingId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e0bdedb-d6c8-49ad-bbf3-8d5ee2d52df6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "466c4c05-2a69-4d64-8a39-e8db4f6b5b04");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82fcd2ee-521a-4cbf-8c15-cae05a702688");

            migrationBuilder.AlterColumn<int>(
                name: "EmployerSettingId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "73c84071-a08e-4563-b7ad-92a713339820", "92f25887-1864-418f-9a62-76aff23b519d", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0911eb8e-d149-46f2-b719-5717be975bd6", "7a404982-4756-4244-8493-e5df6390b8b6", "Employer", "EMPLOYER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d4e12306-e927-4c14-b214-31244fe7a609", "7726b3f6-530c-4b8b-afc6-ad806d7f7e34", "Consultant", "CONSULTANT" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployerSettingId",
                table: "AspNetUsers",
                column: "EmployerSettingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_EmployerSettings_EmployerSettingId",
                table: "AspNetUsers",
                column: "EmployerSettingId",
                principalTable: "EmployerSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
