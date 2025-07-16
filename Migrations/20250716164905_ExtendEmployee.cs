using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering_App.Migrations
{
    /// <inheritdoc />
    public partial class ExtendEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyDepositTotal",
                table: "Employees",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthlyDepositTotal",
                table: "Employees");
        }
    }
}
