using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace incident_management_system.API.Migrations
{
    /// <inheritdoc />
    public partial class CreatesEntityIncidentComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Incidents",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "IncidentComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IncidentId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentComment_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncidentComment_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_CreatedByUserId",
                table: "Incidents",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentComment_IncidentId",
                table: "IncidentComment",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentComment_UserId",
                table: "IncidentComment",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Users_CreatedByUserId",
                table: "Incidents",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Users_CreatedByUserId",
                table: "Incidents");

            migrationBuilder.DropTable(
                name: "IncidentComment");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_CreatedByUserId",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Incidents");
        }
    }
}
