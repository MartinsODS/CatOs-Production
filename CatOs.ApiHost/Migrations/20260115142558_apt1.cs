using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CatOs.ApiHost.Migrations
{
    /// <inheritdoc />
    public partial class apt1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skus",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    SkuCode = table.Column<string>(type: "text", nullable: false),
                    OrdenTable = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skus", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "SkuComponents",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    SkuUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    ComponentUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkuComponents", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_SkuComponents_Components_ComponentUuid",
                        column: x => x.ComponentUuid,
                        principalTable: "Components",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkuComponents_Skus_SkuUuid",
                        column: x => x.SkuUuid,
                        principalTable: "Skus",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkuComponents_ComponentUuid",
                table: "SkuComponents",
                column: "ComponentUuid");

            migrationBuilder.CreateIndex(
                name: "IX_SkuComponents_SkuUuid",
                table: "SkuComponents",
                column: "SkuUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkuComponents");

            migrationBuilder.DropTable(
                name: "Skus");
        }
    }
}
