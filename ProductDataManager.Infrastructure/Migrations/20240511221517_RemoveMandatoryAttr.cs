using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductDataManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMandatoryAttr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mandatory",
                table: "AttributeType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Mandatory",
                table: "AttributeType",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
