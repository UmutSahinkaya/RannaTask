using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RannaTask.DAL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupportForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportForms_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Created", "Email", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(2462), "ahmetsasmaz@gmail.com", "Ahmet", "Şaşmaz" },
                    { 2, new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(2469), "mehmetuzulmez@gmail.com", "Mehmet", "Üzülmez" }
                });

            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "Id", "Created", "Email", "FirstName", "LastName" },
                values: new object[] { 1, new DateTime(2025, 2, 1, 15, 13, 29, 493, DateTimeKind.Local).AddTicks(9926), "manager@gmail.com", "Manager", "" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Code", "Created", "Image", "Name", "Price" },
                values: new object[] { 1, "ABC123", new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(3676), null, "AbcYazılım", 100000m });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "Created", "Name", "Type" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(1613), "Manager", (byte)1 },
                    { 2, new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(1621), "Customer", (byte)2 }
                });

            migrationBuilder.InsertData(
                table: "SupportForms",
                columns: new[] { "Id", "Created", "CustomerId", "Message", "Status", "Subject" },
                values: new object[] { 1, new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(4866), 1, "TEst Deneme", 0, "Test" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "CustomerId", "ManagerId", "Password", "PasswordHash", "RoleId", "Username" },
                values: new object[] { 1, new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(6135), 1, 0, "asdasd", "asdasd", 2, "ahmetsasmaz" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "CustomerId", "ManagerId", "Password", "PasswordHash", "RoleId", "Username" },
                values: new object[] { 2, new DateTime(2025, 2, 1, 15, 13, 29, 494, DateTimeKind.Local).AddTicks(6143), 0, 1, "asdasd", "asdasd", 1, "Manager" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportForms_CustomerId",
                table: "SupportForms",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SupportForms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
