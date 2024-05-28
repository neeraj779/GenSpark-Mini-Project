using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementAPI.Migrations
{
    public partial class AddEmailInTeacherTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 100,
                columns: new[] { "Password", "PasswordHashKey", "RegistrationDate" },
                values: new object[] { new byte[] { 172, 62, 232, 166, 210, 85, 15, 133, 151, 157, 115, 26, 141, 124, 18, 20, 134, 236, 67, 224, 10, 221, 14, 219, 210, 65, 151, 185, 107, 234, 203, 147, 112, 48, 211, 230, 149, 17, 232, 187, 183, 227, 66, 13, 9, 160, 228, 174, 224, 187, 122, 104, 244, 69, 125, 85, 168, 126, 152, 180, 22, 164, 160, 88 }, new byte[] { 119, 20, 63, 42, 150, 149, 49, 118, 145, 25, 40, 173, 76, 225, 193, 184, 97, 19, 60, 74, 137, 162, 253, 112, 211, 69, 26, 16, 40, 34, 13, 187, 95, 76, 209, 83, 195, 240, 114, 166, 232, 67, 13, 146, 185, 22, 236, 185, 128, 0, 35, 192, 201, 226, 242, 230, 19, 37, 200, 246, 132, 157, 1, 163, 60, 90, 139, 15, 61, 255, 113, 129, 136, 65, 135, 5, 118, 31, 62, 21, 85, 107, 33, 79, 145, 195, 7, 127, 41, 108, 5, 112, 57, 149, 215, 82, 168, 157, 74, 162, 71, 182, 87, 242, 89, 190, 4, 46, 80, 220, 78, 103, 86, 151, 84, 158, 100, 132, 131, 39, 214, 175, 254, 240, 71, 26, 236, 147 }, new DateTime(2024, 5, 24, 12, 3, 56, 545, DateTimeKind.Utc).AddTicks(5023) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Teachers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 100,
                columns: new[] { "Email", "Password", "PasswordHashKey", "RegistrationDate" },
                values: new object[] { "admin@presidio.com", new byte[] { 230, 229, 18, 63, 118, 252, 27, 36, 118, 226, 246, 15, 143, 207, 28, 128, 151, 214, 236, 116, 14, 95, 44, 52, 50, 125, 226, 14, 97, 128, 48, 175, 97, 231, 244, 101, 158, 0, 173, 223, 58, 48, 42, 49, 140, 225, 211, 61, 66, 1, 190, 120, 76, 108, 155, 66, 183, 55, 91, 45, 146, 74, 186, 100 }, new byte[] { 250, 199, 38, 9, 219, 16, 144, 83, 248, 117, 45, 185, 226, 87, 226, 148, 85, 68, 45, 209, 163, 116, 22, 213, 160, 115, 185, 101, 205, 107, 140, 12, 20, 217, 59, 121, 84, 176, 18, 86, 235, 24, 182, 68, 179, 62, 44, 186, 67, 165, 0, 120, 189, 226, 119, 232, 50, 64, 190, 151, 91, 246, 174, 89, 74, 250, 109, 232, 71, 142, 231, 3, 234, 5, 90, 49, 228, 134, 231, 216, 160, 190, 156, 86, 67, 194, 30, 217, 34, 1, 68, 172, 153, 58, 238, 159, 61, 123, 77, 93, 222, 45, 189, 23, 217, 10, 136, 165, 193, 199, 204, 203, 18, 125, 27, 182, 78, 152, 9, 111, 241, 134, 53, 158, 203, 151, 64, 176 }, new DateTime(2024, 5, 24, 10, 57, 15, 814, DateTimeKind.Utc).AddTicks(3504) });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }
    }
}
