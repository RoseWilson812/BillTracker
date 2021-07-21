using Microsoft.EntityFrameworkCore.Migrations;

namespace BillTracker.Migrations
{
    public partial class CategoryBillKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryBills",
                table: "CategoryBills");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryBills",
                table: "CategoryBills",
                columns: new[] { "MemberId", "CategoryId", "BillId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryBills",
                table: "CategoryBills");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryBills",
                table: "CategoryBills",
                columns: new[] { "MemberId", "CategoryId" });
        }
    }
}
