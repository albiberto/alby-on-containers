using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlbyOnContainers.ProductDataManager.Migrations
{
    /// <inheritdoc />
    public partial class ThirdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DescrTypeId",
                table: "Descrs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Descrs_DescrTypeId",
                table: "Descrs",
                column: "DescrTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Descrs_DescrTypes_DescrTypeId",
                table: "Descrs",
                column: "DescrTypeId",
                principalTable: "DescrTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Descrs_DescrTypes_DescrTypeId",
                table: "Descrs");

            migrationBuilder.DropIndex(
                name: "IX_Descrs_DescrTypeId",
                table: "Descrs");

            migrationBuilder.DropColumn(
                name: "DescrTypeId",
                table: "Descrs");
        }
    }
}
