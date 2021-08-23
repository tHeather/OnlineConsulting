using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineConsulting.Migrations
{
    public partial class AddRowVersionToConversation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Conversations",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Conversations");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51802d91-7fa7-436c-9873-a201c8a35bfb",
                column: "ConcurrencyStamp",
                value: "df5e56e4-3dbf-43f5-941c-ee23916fd252");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c319ab1e-f914-4ebb-8ac9-d6da40d88419",
                column: "ConcurrencyStamp",
                value: "70f5cedf-9818-459a-a2bd-1a7d3c816c59");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1dbd6ec-4d0e-4f0a-bd9f-125cb168ff42",
                column: "ConcurrencyStamp",
                value: "9a69bd44-bf95-4dd1-ae1c-66c4dc7cc6cb");
        }
    }
}
