using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcShop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Category_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryField",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    FieldId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryField", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Field",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Field", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Product_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductFieldValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    FieldId = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFieldValue", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");
            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
            migrationBuilder.CreateIndex(
                name: "IX_CategoryField_CategoryId",
                table: "CategoryField",
                column: "CategoryId");
            migrationBuilder.AddForeignKey(
                name: "FK_CategoryField_Category_CategoryId",
                table: "CategoryField",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
            migrationBuilder.CreateIndex(
                name: "IX_CategoryField_FieldId",
                table: "CategoryField",
                column: "FieldId");
            migrationBuilder.AddForeignKey(
                name: "FK_CategoryField_Field_FieldId",
                table: "CategoryField",
                column: "FieldId",
                principalTable: "Field",
                principalColumn: "Id");
            migrationBuilder.CreateIndex(
                name: "IX_ProductFieldValue_FieldId",
                table: "ProductFieldValue",
                column: "FieldId");
            migrationBuilder.AddForeignKey(
                name: "FK_ProductFieldValue_Field_FieldId",
                table: "ProductFieldValue",
                column: "FieldId",
                principalTable: "Field",
                principalColumn: "Id");
            migrationBuilder.CreateIndex(
                name: "IX_ProductFieldValue_ProductId",
                table: "ProductFieldValue",
                column: "ProductId");
            migrationBuilder.AddForeignKey(
                name: "FK_ProductFieldValue_Product_ProductId",
                table: "ProductFieldValue",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "CategoryField");

            migrationBuilder.DropTable(
                name: "Field");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "ProductFieldValue");


            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryField_Category_CategoryId",
                table: "CategoryField");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryField_Field_FieldId",
                table: "CategoryField");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFieldValue_Field_FieldId",
                table: "ProductFieldValue");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFieldValue_Product_ProductId",
                table: "ProductFieldValue");

            migrationBuilder.DropIndex(
                name: "IX_Product_CategoryId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_CategoryField_CategoryId",
                table: "CategoryField");

            migrationBuilder.DropIndex(
                name: "IX_CategoryField_FieldId",
                table: "CategoryField");

            migrationBuilder.DropIndex(
                name: "IX_ProductFieldValue_FieldId",
                table: "ProductFieldValue");

            migrationBuilder.DropIndex(
                name: "IX_ProductFieldValue_ProductId",
                table: "ProductFieldValue");
        }
    }
}
