using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductDataManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCluster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attribute_AttributeType_ClusterId",
                table: "Attribute");

            migrationBuilder.RenameColumn(
                name: "ClusterId",
                table: "Attribute",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Attribute_ClusterId",
                table: "Attribute",
                newName: "IX_Attribute_TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attribute_AttributeType_TypeId",
                table: "Attribute",
                column: "TypeId",
                principalTable: "AttributeType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attribute_AttributeType_TypeId",
                table: "Attribute");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Attribute",
                newName: "ClusterId");

            migrationBuilder.RenameIndex(
                name: "IX_Attribute_TypeId",
                table: "Attribute",
                newName: "IX_Attribute_ClusterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attribute_AttributeType_ClusterId",
                table: "Attribute",
                column: "ClusterId",
                principalTable: "AttributeType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
