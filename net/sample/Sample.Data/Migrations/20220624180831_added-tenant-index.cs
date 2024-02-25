using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Data.Migrations
{
    public partial class Addedtenantindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CUSTOMERS_CreatedDate",
                table: "CUSTOMERS",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_CUSTOMERS_Tenant",
                table: "CUSTOMERS",
                column: "Tenant");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CUSTOMERS_CreatedDate",
                table: "CUSTOMERS");

            migrationBuilder.DropIndex(
                name: "IX_CUSTOMERS_Tenant",
                table: "CUSTOMERS");
        }
    }
}
