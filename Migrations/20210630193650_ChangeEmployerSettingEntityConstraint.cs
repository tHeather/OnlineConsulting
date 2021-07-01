using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineConsulting.Migrations
{
    public partial class ChangeEmployerSettingEntityConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubscriptionEndDate",
                table: "EmployerSettings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1a5f5b38-1cc0-486c-bea3-e55d6fd84c36", "d8944bce-8288-4c1d-b41e-8670ff29214c", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ce0b74f1-2af9-445b-961f-345b93c787e9", "5af06007-62bf-4c8e-9b4f-808534d9be30", "Employer", "EMPLOYER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "59f7de6c-e749-4c53-b7c8-96cdb3d703c7", "622a6bcc-bd20-4fbe-99b2-2c01c1ec9649", "Consultant", "CONSULTANT" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a5f5b38-1cc0-486c-bea3-e55d6fd84c36");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "59f7de6c-e749-4c53-b7c8-96cdb3d703c7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce0b74f1-2af9-445b-961f-345b93c787e9");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubscriptionEndDate",
                table: "EmployerSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

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
        }
    }
}
