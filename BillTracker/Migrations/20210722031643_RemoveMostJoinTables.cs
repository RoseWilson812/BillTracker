using Microsoft.EntityFrameworkCore.Migrations;

namespace BillTracker.Migrations
{
    public partial class RemoveMostJoinTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryBills");

            migrationBuilder.DropTable(
                name: "MemberBills");

            migrationBuilder.DropTable(
                name: "MemberCategorys");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Categorys",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "Bills",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Bills",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MemberCategoryBills",
                columns: table => new
                {
                    MemberId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    BillId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberCategoryBills", x => new { x.MemberId, x.CategoryId, x.BillId });
                    table.ForeignKey(
                        name: "FK_MemberCategoryBills_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberCategoryBills_Categorys_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categorys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberCategoryBills_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberCategoryBills_BillId",
                table: "MemberCategoryBills",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberCategoryBills_CategoryId",
                table: "MemberCategoryBills",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberCategoryBills");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Categorys");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bills");

            migrationBuilder.CreateTable(
                name: "CategoryBills",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    BillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryBills", x => new { x.MemberId, x.CategoryId, x.BillId });
                    table.ForeignKey(
                        name: "FK_CategoryBills_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryBills_Categorys_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categorys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryBills_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberBills",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    BillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberBills", x => new { x.MemberId, x.BillId });
                    table.ForeignKey(
                        name: "FK_MemberBills_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberBills_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberCategorys",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberCategorys", x => new { x.MemberId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_MemberCategorys_Categorys_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categorys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberCategorys_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryBills_BillId",
                table: "CategoryBills",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryBills_CategoryId",
                table: "CategoryBills",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberBills_BillId",
                table: "MemberBills",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberCategorys_CategoryId",
                table: "MemberCategorys",
                column: "CategoryId");
        }
    }
}
