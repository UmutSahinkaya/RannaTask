using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RannaTask.DAL.Migrations
{
    public partial class UserTableAddedCustomersManagers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ManagerId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 2, 1, 17, 6, 15, 899, DateTimeKind.Local).AddTicks(4014));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2025, 2, 1, 17, 6, 15, 899, DateTimeKind.Local).AddTicks(4019));

            migrationBuilder.UpdateData(
                table: "Managers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 2, 1, 17, 6, 15, 899, DateTimeKind.Local).AddTicks(2381));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 2, 1, 17, 6, 15, 899, DateTimeKind.Local).AddTicks(4659));

            migrationBuilder.UpdateData(
                table: "SupportForms",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 2, 1, 17, 6, 15, 899, DateTimeKind.Local).AddTicks(5435));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 2, 1, 17, 6, 15, 899, DateTimeKind.Local).AddTicks(3377));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2025, 2, 1, 17, 6, 15, 899, DateTimeKind.Local).AddTicks(3383));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "ManagerId" },
                values: new object[] { new DateTime(2025, 2, 1, 17, 6, 15, 899, DateTimeKind.Local).AddTicks(6168), null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "CustomerId" },
                values: new object[] { new DateTime(2025, 2, 1, 17, 6, 15, 899, DateTimeKind.Local).AddTicks(6174), null });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CustomerId",
                table: "Users",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ManagerId",
                table: "Users",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Customers_CustomerId",
                table: "Users",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Managers_ManagerId",
                table: "Users",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Customers_CustomerId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Managers_ManagerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CustomerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ManagerId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "ManagerId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(2462));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(2469));

            migrationBuilder.UpdateData(
                table: "Managers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 2, 1, 15, 13, 29, 493, DateTimeKind.Local).AddTicks(9926));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(3676));

            migrationBuilder.UpdateData(
                table: "SupportForms",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(4866));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(1613));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(1621));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "ManagerId" },
                values: new object[] { new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(6135), 0 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "CustomerId" },
                values: new object[] { new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(6143), 0 });
        }
    }
}
