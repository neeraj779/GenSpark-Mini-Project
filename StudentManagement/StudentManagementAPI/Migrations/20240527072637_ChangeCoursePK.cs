using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementAPI.Migrations
{
    public partial class ChangeCoursePK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseOfferings_Courses_CourseId",
                table: "CourseOfferings");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Courses_CourseId",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_CourseId",
                table: "Enrollments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_CourseOfferings_CourseId",
                table: "CourseOfferings");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_CourseId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Assignments");

            migrationBuilder.AddColumn<string>(
                name: "CourseCode",
                table: "Enrollments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CourseCode",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "CourseCode",
                table: "CourseOfferings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CourseCode",
                table: "Assignments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "CourseCode");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 100,
                columns: new[] { "Password", "PasswordHashKey", "RegistrationDate" },
                values: new object[] { new byte[] { 112, 137, 152, 199, 58, 46, 132, 70, 159, 73, 127, 25, 231, 134, 187, 177, 173, 222, 210, 109, 175, 64, 127, 162, 251, 146, 183, 248, 173, 114, 161, 93, 78, 134, 135, 181, 199, 195, 114, 14, 186, 48, 197, 98, 128, 237, 205, 23, 49, 244, 20, 172, 91, 136, 75, 57, 68, 46, 101, 110, 193, 246, 217, 91 }, new byte[] { 125, 231, 122, 54, 81, 228, 250, 137, 3, 5, 3, 200, 139, 61, 192, 187, 16, 208, 101, 56, 157, 216, 153, 210, 131, 144, 136, 187, 197, 244, 20, 239, 46, 44, 162, 33, 133, 81, 171, 166, 133, 140, 162, 122, 216, 142, 112, 105, 69, 59, 39, 154, 25, 171, 181, 118, 105, 251, 23, 80, 147, 152, 54, 253, 140, 34, 228, 217, 231, 171, 13, 199, 56, 12, 238, 88, 131, 53, 247, 194, 239, 157, 52, 18, 117, 51, 91, 47, 70, 45, 108, 99, 252, 54, 64, 63, 111, 237, 237, 207, 187, 133, 86, 192, 157, 76, 253, 159, 172, 23, 152, 74, 232, 247, 137, 116, 178, 155, 123, 21, 119, 75, 18, 249, 65, 135, 47, 220 }, new DateTime(2024, 5, 27, 7, 26, 36, 832, DateTimeKind.Utc).AddTicks(9528) });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseCode",
                table: "Enrollments",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_CourseOfferings_CourseCode",
                table: "CourseOfferings",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CourseCode",
                table: "Assignments",
                column: "CourseCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Courses_CourseCode",
                table: "Assignments",
                column: "CourseCode",
                principalTable: "Courses",
                principalColumn: "CourseCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOfferings_Courses_CourseCode",
                table: "CourseOfferings",
                column: "CourseCode",
                principalTable: "Courses",
                principalColumn: "CourseCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Courses_CourseCode",
                table: "Enrollments",
                column: "CourseCode",
                principalTable: "Courses",
                principalColumn: "CourseCode",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Courses_CourseCode",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseOfferings_Courses_CourseCode",
                table: "CourseOfferings");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Courses_CourseCode",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_CourseCode",
                table: "Enrollments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_CourseOfferings_CourseCode",
                table: "CourseOfferings");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_CourseCode",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "CourseCode",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "CourseCode",
                table: "CourseOfferings");

            migrationBuilder.DropColumn(
                name: "CourseCode",
                table: "Assignments");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Enrollments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CourseCode",
                table: "Courses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Assignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "CourseId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 100,
                columns: new[] { "Password", "PasswordHashKey", "RegistrationDate" },
                values: new object[] { new byte[] { 172, 62, 232, 166, 210, 85, 15, 133, 151, 157, 115, 26, 141, 124, 18, 20, 134, 236, 67, 224, 10, 221, 14, 219, 210, 65, 151, 185, 107, 234, 203, 147, 112, 48, 211, 230, 149, 17, 232, 187, 183, 227, 66, 13, 9, 160, 228, 174, 224, 187, 122, 104, 244, 69, 125, 85, 168, 126, 152, 180, 22, 164, 160, 88 }, new byte[] { 119, 20, 63, 42, 150, 149, 49, 118, 145, 25, 40, 173, 76, 225, 193, 184, 97, 19, 60, 74, 137, 162, 253, 112, 211, 69, 26, 16, 40, 34, 13, 187, 95, 76, 209, 83, 195, 240, 114, 166, 232, 67, 13, 146, 185, 22, 236, 185, 128, 0, 35, 192, 201, 226, 242, 230, 19, 37, 200, 246, 132, 157, 1, 163, 60, 90, 139, 15, 61, 255, 113, 129, 136, 65, 135, 5, 118, 31, 62, 21, 85, 107, 33, 79, 145, 195, 7, 127, 41, 108, 5, 112, 57, 149, 215, 82, 168, 157, 74, 162, 71, 182, 87, 242, 89, 190, 4, 46, 80, 220, 78, 103, 86, 151, 84, 158, 100, 132, 131, 39, 214, 175, 254, 240, 71, 26, 236, 147 }, new DateTime(2024, 5, 24, 12, 3, 56, 545, DateTimeKind.Utc).AddTicks(5023) });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseId",
                table: "Enrollments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseOfferings_CourseId",
                table: "CourseOfferings",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CourseId",
                table: "Assignments",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOfferings_Courses_CourseId",
                table: "CourseOfferings",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Courses_CourseId",
                table: "Enrollments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
