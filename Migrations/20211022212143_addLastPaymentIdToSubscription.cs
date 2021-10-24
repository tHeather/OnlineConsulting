using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineConsulting.Migrations
{
    public partial class addLastPaymentIdToSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_EmployeeId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_EmployeeId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Payments");

            migrationBuilder.AddColumn<Guid>(
                name: "LastPaymentId",
                table: "Subscriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "EmployerId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Payments_EmployerId",
                table: "Payments",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SubscriptionTypeId",
                table: "Payments",
                column: "SubscriptionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_EmployerId",
                table: "Payments",
                column: "EmployerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_SubscriptionTypes_SubscriptionTypeId",
                table: "Payments",
                column: "SubscriptionTypeId",
                principalTable: "SubscriptionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_EmployerId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_SubscriptionTypes_SubscriptionTypeId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_EmployerId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_SubscriptionTypeId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "LastPaymentId",
                table: "Subscriptions");

            migrationBuilder.AlterColumn<string>(
                name: "EmployerId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51802d91-7fa7-436c-9873-a201c8a35bfb",
                column: "ConcurrencyStamp",
                value: "2ea30054-1f59-432b-b19e-e3ea4ef2b112");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c319ab1e-f914-4ebb-8ac9-d6da40d88419",
                column: "ConcurrencyStamp",
                value: "91312b98-f340-481a-88ca-caaf36e26afc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1dbd6ec-4d0e-4f0a-bd9f-125cb168ff42",
                column: "ConcurrencyStamp",
                value: "0acacf95-0fe8-4c03-a286-2e2d5f4b2287");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7f10381c-7a7c-4e65-b468-a5b32c789720",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "07a21e7e-f8d8-4d99-b0a1-8581e115f429", "7ef60378-1139-4e07-aba1-a0c085f2343c" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_EmployeeId",
                table: "Payments",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_EmployeeId",
                table: "Payments",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
