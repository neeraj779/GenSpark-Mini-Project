using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementAPI.Migrations
{
    public partial class UpdateStududentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseOfferings_Branches_BranchId",
                table: "CourseOfferings");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Branches_BranchId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Degrees_DegreeId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Degrees");

            migrationBuilder.DropIndex(
                name: "IX_Students_BranchId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_DegreeId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_CourseOfferings_BranchId",
                table: "CourseOfferings");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DegreeId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "CourseOfferings");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Students",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 100,
                columns: new[] { "Password", "PasswordHashKey", "RegistrationDate" },
                values: new object[] { new byte[] { 159, 216, 196, 161, 243, 208, 135, 252, 234, 143, 90, 237, 182, 220, 92, 168, 20, 161, 173, 136, 255, 17, 170, 194, 134, 218, 131, 2, 109, 52, 121, 211, 27, 21, 12, 178, 11, 48, 67, 173, 17, 213, 244, 78, 178, 239, 120, 253, 59, 122, 224, 50, 149, 61, 95, 52, 97, 159, 137, 59, 225, 44, 229, 212 }, new byte[] { 73, 206, 85, 4, 151, 111, 104, 104, 111, 235, 168, 41, 66, 16, 214, 146, 224, 67, 203, 0, 209, 13, 60, 19, 127, 11, 30, 8, 158, 84, 18, 39, 152, 153, 132, 7, 2, 232, 18, 8, 94, 203, 218, 202, 133, 202, 200, 197, 2, 42, 237, 94, 225, 98, 112, 108, 216, 214, 231, 160, 223, 87, 67, 65, 37, 151, 140, 130, 35, 73, 236, 6, 82, 55, 41, 75, 180, 100, 108, 181, 238, 132, 142, 5, 171, 69, 17, 70, 207, 242, 149, 107, 128, 35, 221, 145, 16, 147, 35, 79, 205, 171, 41, 48, 11, 129, 210, 159, 210, 95, 229, 73, 12, 166, 77, 119, 46, 105, 190, 210, 26, 239, 254, 61, 185, 250, 109, 162 }, new DateTime(2024, 5, 27, 10, 43, 19, 405, DateTimeKind.Utc).AddTicks(240) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Students",
                type: "int",
                maxLength: 150,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DegreeId",
                table: "Students",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "CourseOfferings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Degrees",
                columns: table => new
                {
                    DegreeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DegreeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degrees", x => x.DegreeId);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DegreeId = table.Column<int>(type: "int", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.BranchId);
                    table.ForeignKey(
                        name: "FK_Branches_Degrees_DegreeId",
                        column: x => x.DegreeId,
                        principalTable: "Degrees",
                        principalColumn: "DegreeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 100,
                columns: new[] { "Password", "PasswordHashKey", "RegistrationDate" },
                values: new object[] { new byte[] { 112, 137, 152, 199, 58, 46, 132, 70, 159, 73, 127, 25, 231, 134, 187, 177, 173, 222, 210, 109, 175, 64, 127, 162, 251, 146, 183, 248, 173, 114, 161, 93, 78, 134, 135, 181, 199, 195, 114, 14, 186, 48, 197, 98, 128, 237, 205, 23, 49, 244, 20, 172, 91, 136, 75, 57, 68, 46, 101, 110, 193, 246, 217, 91 }, new byte[] { 125, 231, 122, 54, 81, 228, 250, 137, 3, 5, 3, 200, 139, 61, 192, 187, 16, 208, 101, 56, 157, 216, 153, 210, 131, 144, 136, 187, 197, 244, 20, 239, 46, 44, 162, 33, 133, 81, 171, 166, 133, 140, 162, 122, 216, 142, 112, 105, 69, 59, 39, 154, 25, 171, 181, 118, 105, 251, 23, 80, 147, 152, 54, 253, 140, 34, 228, 217, 231, 171, 13, 199, 56, 12, 238, 88, 131, 53, 247, 194, 239, 157, 52, 18, 117, 51, 91, 47, 70, 45, 108, 99, 252, 54, 64, 63, 111, 237, 237, 207, 187, 133, 86, 192, 157, 76, 253, 159, 172, 23, 152, 74, 232, 247, 137, 116, 178, 155, 123, 21, 119, 75, 18, 249, 65, 135, 47, 220 }, new DateTime(2024, 5, 27, 7, 26, 36, 832, DateTimeKind.Utc).AddTicks(9528) });

            migrationBuilder.CreateIndex(
                name: "IX_Students_BranchId",
                table: "Students",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_DegreeId",
                table: "Students",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseOfferings_BranchId",
                table: "CourseOfferings",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_DegreeId",
                table: "Branches",
                column: "DegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOfferings_Branches_BranchId",
                table: "CourseOfferings",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Branches_BranchId",
                table: "Students",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Degrees_DegreeId",
                table: "Students",
                column: "DegreeId",
                principalTable: "Degrees",
                principalColumn: "DegreeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
