using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementAPI.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CourseCredit = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseCode);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordHashKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Role = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DueDate = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_Assignments_Courses_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "Courses",
                        principalColumn: "CourseCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RollNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "Date", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "Date", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.TeacherId);
                    table.ForeignKey(
                        name: "FK_Teachers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    EnrollmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.EnrollmentId);
                    table.ForeignKey(
                        name: "FK_Enrollments_Courses_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "Courses",
                        principalColumn: "CourseCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    SubmissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "Date", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.SubmissionId);
                    table.ForeignKey(
                        name: "FK_Submissions_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "AssignmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submissions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseOfferings",
                columns: table => new
                {
                    CourseOfferingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseOfferings", x => x.CourseOfferingId);
                    table.ForeignKey(
                        name: "FK_CourseOfferings_Courses_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "Courses",
                        principalColumn: "CourseCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseOfferings_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseOfferingId = table.Column<int>(type: "int", nullable: false),
                    ClassDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassId);
                    table.ForeignKey(
                        name: "FK_Classes_CourseOfferings_CourseOfferingId",
                        column: x => x.CourseOfferingId,
                        principalTable: "CourseOfferings",
                        principalColumn: "CourseOfferingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassAttendances",
                columns: table => new
                {
                    AttendanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "Date", nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassAttendances", x => x.AttendanceId);
                    table.ForeignKey(
                        name: "FK_ClassAttendances_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassAttendances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseCode", "CourseCredit", "CourseName" },
                values: new object[,]
                {
                    { "CSE101", 3, "Introduction to Computer Science" },
                    { "CSE102", 4, "Data Structures" },
                    { "CSE103", 2, "Algorithms" },
                    { "CSE104", 3, "Database Management Systems" },
                    { "CSE105", 2, "Operating Systems" },
                    { "CSE106", 2, "Computer Networks" },
                    { "CSE107", 4, "Software Engineering" },
                    { "CSE108", 3, "Web Development" },
                    { "CSE109", 4, "Artificial Intelligence" },
                    { "CSE110", 3, "Machine Learning" },
                    { "CSE201", 3, "Object-Oriented Programming" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "DateOfBirth", "Department", "Email", "FullName", "Gender", "Phone", "RollNo", "Status", "UserId" },
                values: new object[,]
                {
                    { 4000, new DateTime(1998, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Computer Science", "raj.patel@gmail.com", "Mr. Raj Patel", "Male", "9374729562", "CSE2020002", 0, null },
                    { 4001, new DateTime(2000, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Electronics and Communication", "neha.desai@gmail.com", "Ms. Neha Desai", "Female", "9374729563", "ECE2020001", 0, null },
                    { 4002, new DateTime(1997, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mechanical Engineering", "amit.sharma@gmail.com", "Mr. Amit Sharma", "Male", "9374729564", "ME2020001", 0, null },
                    { 4003, new DateTime(1999, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Electrical Engineering", "sneha.rao@gmail.com", "Ms. Sneha Rao", "Female", "9374729565", "EE2020001", 0, null },
                    { 4004, new DateTime(1996, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Civil Engineering", "vivek.gupta@gmail.com", "Mr. Vivek Gupta", "Male", "9374729566", "CE2020001", 0, null }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "TeacherId", "DateOfBirth", "Email", "FullName", "Gender", "Phone", "UserId" },
                values: new object[,]
                {
                    { 2000, new DateTime(1978, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "sunita.verma@gmail.com", "Dr. Sunita Verma", "Female", "9876543212", null },
                    { 2001, new DateTime(1975, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "ramesh.gupta@gmail.com", "Mr. Ramesh Gupta", "Male", "9876543213", null },
                    { 2002, new DateTime(1982, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "anjali.mehta@gmail.com", "Ms. Anjali Mehta", "Female", "9876543214", null },
                    { 2003, new DateTime(1970, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "vijay.patil@gmail.com", "Mr. Vijay Patil", "Male", "9876543215", null },
                    { 2004, new DateTime(1983, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "seema.reddy@gmail.com", "Dr. Seema Reddy", "Female", "9876543216", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Password", "PasswordHashKey", "RegistrationDate", "Role", "Status", "UserName" },
                values: new object[] { 100, new byte[] { 26, 220, 122, 133, 151, 198, 222, 56, 243, 177, 120, 180, 13, 123, 58, 7, 98, 122, 119, 252, 61, 74, 192, 68, 129, 68, 125, 197, 216, 99, 190, 48, 127, 69, 18, 108, 243, 208, 27, 59, 59, 68, 200, 84, 101, 105, 63, 155, 166, 18, 17, 222, 77, 251, 0, 47, 112, 73, 45, 228, 218, 79, 18, 239 }, new byte[] { 198, 90, 132, 88, 220, 42, 242, 68, 66, 203, 177, 227, 147, 176, 252, 217, 241, 138, 66, 173, 93, 37, 182, 253, 242, 13, 42, 227, 176, 197, 3, 10, 15, 47, 41, 195, 21, 16, 230, 137, 249, 38, 239, 104, 212, 227, 140, 223, 59, 80, 105, 255, 149, 40, 206, 155, 175, 135, 63, 51, 117, 211, 50, 225, 73, 135, 246, 108, 22, 65, 254, 38, 115, 212, 11, 157, 147, 71, 123, 225, 186, 133, 71, 44, 166, 197, 175, 78, 216, 229, 180, 152, 141, 99, 158, 65, 136, 6, 138, 111, 200, 74, 63, 176, 184, 235, 166, 88, 15, 245, 60, 242, 138, 95, 156, 200, 229, 17, 98, 183, 242, 79, 127, 118, 228, 60, 88, 109 }, new DateTime(2024, 5, 30, 4, 59, 1, 1, DateTimeKind.Utc).AddTicks(2192), 0, "Active", "admin" });

            migrationBuilder.InsertData(
                table: "CourseOfferings",
                columns: new[] { "CourseOfferingId", "CourseCode", "TeacherId" },
                values: new object[,]
                {
                    { 1, "CSE101", 2000 },
                    { 2, "CSE102", 2001 },
                    { 3, "CSE103", 2002 },
                    { 4, "CSE104", 2003 },
                    { 5, "CSE105", 2004 }
                });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "EnrollmentId", "CourseCode", "EnrollmentDate", "StudentId" },
                values: new object[,]
                {
                    { 1, "CSE101", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4000 },
                    { 2, "CSE102", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4000 },
                    { 3, "CSE103", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4000 },
                    { 4, "CSE101", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4001 },
                    { 5, "CSE102", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4001 },
                    { 6, "CSE103", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4001 },
                    { 7, "CSE101", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4002 },
                    { 8, "CSE102", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4002 },
                    { 9, "CSE103", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4003 },
                    { 10, "CSE101", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4003 },
                    { 11, "CSE102", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4003 },
                    { 12, "CSE103", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4003 },
                    { 13, "CSE101", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4004 },
                    { 14, "CSE102", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4004 },
                    { 15, "CSE103", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4004 }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "ClassId", "ClassDateAndTime", "CourseOfferingId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 5, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, new DateTime(2024, 5, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, new DateTime(2024, 5, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 4, new DateTime(2024, 5, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 5, new DateTime(2024, 5, 14, 9, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 6, new DateTime(2024, 5, 7, 2, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 7, new DateTime(2024, 5, 9, 11, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 8, new DateTime(2024, 5, 11, 13, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 9, new DateTime(2024, 5, 13, 14, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 10, new DateTime(2024, 5, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CourseCode",
                table: "Assignments",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_ClassAttendances_ClassId",
                table: "ClassAttendances",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassAttendances_StudentId",
                table: "ClassAttendances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CourseOfferingId",
                table: "Classes",
                column: "CourseOfferingId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseOfferings_CourseCode",
                table: "CourseOfferings",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_CourseOfferings_TeacherId",
                table: "CourseOfferings",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseCode",
                table: "Enrollments",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId",
                table: "Enrollments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_AssignmentId",
                table: "Submissions",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_StudentId",
                table: "Submissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserId",
                table: "Teachers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassAttendances");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "CourseOfferings");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
