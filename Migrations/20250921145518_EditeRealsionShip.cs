using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Carrer.Migrations
{
    /// <inheritdoc />
    public partial class EditeRealsionShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificate_step");

            migrationBuilder.DropTable(
                name: "Employee_Skill");

            migrationBuilder.CreateTable(
                name: "CertificatesSkills",
                columns: table => new
                {
                    CertificatesId = table.Column<int>(type: "integer", nullable: false),
                    SkillsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificatesSkills", x => new { x.CertificatesId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_CertificatesSkills_Certificates_CertificatesId",
                        column: x => x.CertificatesId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificatesSkills_Skills_SkillsId",
                        column: x => x.SkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CertificatesSkills_SkillsId",
                table: "CertificatesSkills",
                column: "SkillsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificatesSkills");

            migrationBuilder.CreateTable(
                name: "Certificate_step",
                columns: table => new
                {
                    CertificateId = table.Column<int>(type: "integer", nullable: false),
                    StepId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificate_step", x => new { x.CertificateId, x.StepId });
                    table.ForeignKey(
                        name: "FK_Certificate_step_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Certificate_step_Steps_StepId",
                        column: x => x.StepId,
                        principalTable: "Steps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee_Skill",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee_Skill", x => new { x.EmployeeId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_Employee_Skill_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_Skill_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_step_StepId",
                table: "Certificate_step",
                column: "StepId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Skill_SkillId",
                table: "Employee_Skill",
                column: "SkillId");
        }
    }
}
