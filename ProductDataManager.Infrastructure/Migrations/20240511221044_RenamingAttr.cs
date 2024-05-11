using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductDataManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamingAttr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttributeType_AttributeCluster_ClusterId",
                table: "AttributeType");

            migrationBuilder.DropTable(
                name: "AttributeCluster");

            migrationBuilder.DropIndex(
                name: "IX_AttributeType_ClusterId",
                table: "AttributeType");

            migrationBuilder.DropColumn(
                name: "ClusterId",
                table: "AttributeType");

            migrationBuilder.AddColumn<bool>(
                name: "Mandatory",
                table: "AttributeType",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Tech",
                table: "AttributeType",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Attribute",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClusterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attribute_AttributeType_ClusterId",
                        column: x => x.ClusterId,
                        principalTable: "AttributeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attribute_ClusterId",
                table: "Attribute",
                column: "ClusterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attribute");

            migrationBuilder.DropColumn(
                name: "Mandatory",
                table: "AttributeType");

            migrationBuilder.DropColumn(
                name: "Tech",
                table: "AttributeType");

            migrationBuilder.AddColumn<Guid>(
                name: "ClusterId",
                table: "AttributeType",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AttributeCluster",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Mandatory = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Tech = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeCluster", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeType_ClusterId",
                table: "AttributeType",
                column: "ClusterId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeType_AttributeCluster_ClusterId",
                table: "AttributeType",
                column: "ClusterId",
                principalTable: "AttributeCluster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
