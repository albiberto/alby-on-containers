using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductDataManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "DescriptionTypes");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DescriptionValues",
                newName: "Value");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DescriptionTypes",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "DescriptionTypes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_DescriptionTypes_Name",
                table: "DescriptionTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DescriptionTypes_Name",
                table: "DescriptionTypes");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "DescriptionValues",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DescriptionTypes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "DescriptionTypes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "DescriptionTypes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
