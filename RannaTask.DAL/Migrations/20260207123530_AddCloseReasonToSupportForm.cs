using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RannaTask.DAL.Migrations
{
    public partial class AddCloseReasonToSupportForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CloseReason",
                table: "SupportForms",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseReason",
                table: "SupportForms");
        }
    }
}
