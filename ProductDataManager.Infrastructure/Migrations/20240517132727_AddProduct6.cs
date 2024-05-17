using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductDataManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProduct6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDescription_DescriptionValues_DescriptionId",
                table: "ProductDescription");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDescription_Products_ProductId",
                table: "ProductDescription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductDescription",
                table: "ProductDescription");

            migrationBuilder.RenameTable(
                name: "ProductDescription",
                newName: "ProductsDescriptions");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDescription_ProductId",
                table: "ProductsDescriptions",
                newName: "IX_ProductsDescriptions_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDescription_DescriptionId",
                table: "ProductsDescriptions",
                newName: "IX_ProductsDescriptions_DescriptionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsDescriptions",
                table: "ProductsDescriptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsDescriptions_DescriptionValues_DescriptionId",
                table: "ProductsDescriptions",
                column: "DescriptionId",
                principalTable: "DescriptionValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsDescriptions_Products_ProductId",
                table: "ProductsDescriptions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsDescriptions_DescriptionValues_DescriptionId",
                table: "ProductsDescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsDescriptions_Products_ProductId",
                table: "ProductsDescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsDescriptions",
                table: "ProductsDescriptions");

            migrationBuilder.RenameTable(
                name: "ProductsDescriptions",
                newName: "ProductDescription");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsDescriptions_ProductId",
                table: "ProductDescription",
                newName: "IX_ProductDescription_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsDescriptions_DescriptionId",
                table: "ProductDescription",
                newName: "IX_ProductDescription_DescriptionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDescription",
                table: "ProductDescription",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDescription_DescriptionValues_DescriptionId",
                table: "ProductDescription",
                column: "DescriptionId",
                principalTable: "DescriptionValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDescription_Products_ProductId",
                table: "ProductDescription",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
