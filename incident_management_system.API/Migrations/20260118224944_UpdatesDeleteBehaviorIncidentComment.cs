using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace incident_management_system.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatesDeleteBehaviorIncidentComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncidentComment_Incidents_IncidentId",
                table: "IncidentComment");

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentComment_Incidents_IncidentId",
                table: "IncidentComment",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncidentComment_Incidents_IncidentId",
                table: "IncidentComment");

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentComment_Incidents_IncidentId",
                table: "IncidentComment",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
