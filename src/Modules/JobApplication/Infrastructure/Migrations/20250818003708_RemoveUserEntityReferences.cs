using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserEntityReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_job_applications_users_applicant_id",
                schema: "application",
                table: "job_applications");

            migrationBuilder.DropTable(
                name: "users",
                schema: "application");

            migrationBuilder.DropIndex(
                name: "ix_job_applications_applicant_id",
                schema: "application",
                table: "job_applications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                schema: "application",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_job_applications_applicant_id",
                schema: "application",
                table: "job_applications",
                column: "applicant_id");

            migrationBuilder.AddForeignKey(
                name: "fk_job_applications_users_applicant_id",
                schema: "application",
                table: "job_applications",
                column: "applicant_id",
                principalSchema: "application",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
