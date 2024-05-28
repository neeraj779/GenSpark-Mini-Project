﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace StudentManagementAPI.Migrations
{
    [DbContext(typeof(StudentManagementContext))]
    [Migration("20240527072637_ChangeCoursePK")]
    partial class ChangeCoursePK
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.30")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Assignment", b =>
                {
                    b.Property<int>("AssignmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AssignmentId"), 1L, 1);

                    b.Property<string>("CourseCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("AssignmentId");

                    b.HasIndex("CourseCode");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Branch", b =>
                {
                    b.Property<int>("BranchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BranchId"), 1L, 1);

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("DegreeId")
                        .HasColumnType("int");

                    b.HasKey("BranchId");

                    b.HasIndex("DegreeId");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Class", b =>
                {
                    b.Property<int>("ClassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClassId"), 1L, 1);

                    b.Property<int>("CourseOfferingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Schedule")
                        .HasColumnType("datetime2");

                    b.HasKey("ClassId");

                    b.HasIndex("CourseOfferingId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.ClassAttendance", b =>
                {
                    b.Property<int>("AttendanceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AttendanceId"), 1L, 1);

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("AttendanceId");

                    b.HasIndex("ClassId");

                    b.HasIndex("StudentId");

                    b.ToTable("ClassAttendances");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Course", b =>
                {
                    b.Property<string>("CourseCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CourseCredit")
                        .HasColumnType("int");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CourseCode");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.CourseOffering", b =>
                {
                    b.Property<int>("CourseOfferingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseOfferingId"), 1L, 1);

                    b.Property<int>("BranchId")
                        .HasColumnType("int");

                    b.Property<string>("CourseCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int?>("TeacherId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("CourseOfferingId");

                    b.HasIndex("BranchId");

                    b.HasIndex("CourseCode");

                    b.HasIndex("TeacherId");

                    b.ToTable("CourseOfferings");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Degree", b =>
                {
                    b.Property<int>("DegreeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DegreeId"), 1L, 1);

                    b.Property<string>("DegreeName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("DegreeId");

                    b.ToTable("Degrees");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Enrollment", b =>
                {
                    b.Property<int>("EnrollmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EnrollmentId"), 1L, 1);

                    b.Property<string>("CourseCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("EnrollmentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("EnrollmentId");

                    b.HasIndex("CourseCode");

                    b.HasIndex("StudentId");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentId"), 1L, 1);

                    b.Property<int>("BranchId")
                        .HasMaxLength(150)
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<int>("DegreeId")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RollNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("StudentId");

                    b.HasIndex("BranchId");

                    b.HasIndex("DegreeId");

                    b.HasIndex("UserId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Submission", b =>
                {
                    b.Property<int>("SubmissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubmissionId"), 1L, 1);

                    b.Property<int>("AssignmentId")
                        .HasColumnType("int");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("SubmissionId");

                    b.HasIndex("AssignmentId");

                    b.HasIndex("StudentId");

                    b.ToTable("Submissions");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Teacher", b =>
                {
                    b.Property<int>("TeacherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TeacherId"), 1L, 1);

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TeacherId");

                    b.HasIndex("UserId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordHashKey")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime");

                    b.Property<int>("Role")
                        .HasMaxLength(20)
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 100,
                            Password = new byte[] { 112, 137, 152, 199, 58, 46, 132, 70, 159, 73, 127, 25, 231, 134, 187, 177, 173, 222, 210, 109, 175, 64, 127, 162, 251, 146, 183, 248, 173, 114, 161, 93, 78, 134, 135, 181, 199, 195, 114, 14, 186, 48, 197, 98, 128, 237, 205, 23, 49, 244, 20, 172, 91, 136, 75, 57, 68, 46, 101, 110, 193, 246, 217, 91 },
                            PasswordHashKey = new byte[] { 125, 231, 122, 54, 81, 228, 250, 137, 3, 5, 3, 200, 139, 61, 192, 187, 16, 208, 101, 56, 157, 216, 153, 210, 131, 144, 136, 187, 197, 244, 20, 239, 46, 44, 162, 33, 133, 81, 171, 166, 133, 140, 162, 122, 216, 142, 112, 105, 69, 59, 39, 154, 25, 171, 181, 118, 105, 251, 23, 80, 147, 152, 54, 253, 140, 34, 228, 217, 231, 171, 13, 199, 56, 12, 238, 88, 131, 53, 247, 194, 239, 157, 52, 18, 117, 51, 91, 47, 70, 45, 108, 99, 252, 54, 64, 63, 111, 237, 237, 207, 187, 133, 86, 192, 157, 76, 253, 159, 172, 23, 152, 74, 232, 247, 137, 116, 178, 155, 123, 21, 119, 75, 18, 249, 65, 135, 47, 220 },
                            RegistrationDate = new DateTime(2024, 5, 27, 7, 26, 36, 832, DateTimeKind.Utc).AddTicks(9528),
                            Role = 0,
                            Status = "Active",
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Assignment", b =>
                {
                    b.HasOne("StudentManagementAPI.Models.DBModels.Course", "Course")
                        .WithMany("Assignments")
                        .HasForeignKey("CourseCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Branch", b =>
                {
                    b.HasOne("StudentManagementAPI.Models.DBModels.Degree", "Degree")
                        .WithMany("Branches")
                        .HasForeignKey("DegreeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Degree");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Class", b =>
                {
                    b.HasOne("StudentManagementAPI.Models.DBModels.CourseOffering", "CourseOffering")
                        .WithMany("Classes")
                        .HasForeignKey("CourseOfferingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CourseOffering");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.ClassAttendance", b =>
                {
                    b.HasOne("StudentManagementAPI.Models.DBModels.Class", "Class")
                        .WithMany("ClassAttendances")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudentManagementAPI.Models.DBModels.Student", "Student")
                        .WithMany("ClassAttendances")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.CourseOffering", b =>
                {
                    b.HasOne("StudentManagementAPI.Models.DBModels.Branch", "Branch")
                        .WithMany("CourseOfferings")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManagementAPI.Models.DBModels.Course", "Course")
                        .WithMany("CourseOfferings")
                        .HasForeignKey("CourseCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManagementAPI.Models.DBModels.Teacher", "Teacher")
                        .WithMany("CourseOfferings")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("Course");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Enrollment", b =>
                {
                    b.HasOne("StudentManagementAPI.Models.DBModels.Course", "Course")
                        .WithMany("Enrollments")
                        .HasForeignKey("CourseCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudentManagementAPI.Models.DBModels.Student", "Student")
                        .WithMany("Enrollments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Student", b =>
                {
                    b.HasOne("StudentManagementAPI.Models.DBModels.Branch", "Branch")
                        .WithMany("Students")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudentManagementAPI.Models.DBModels.Degree", "Degree")
                        .WithMany("Students")
                        .HasForeignKey("DegreeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudentManagementAPI.Models.DBModels.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Branch");

                    b.Navigation("Degree");

                    b.Navigation("User");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Submission", b =>
                {
                    b.HasOne("StudentManagementAPI.Models.DBModels.Assignment", "Assignment")
                        .WithMany("Submissions")
                        .HasForeignKey("AssignmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudentManagementAPI.Models.DBModels.Student", "Student")
                        .WithMany("Submissions")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Assignment");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Teacher", b =>
                {
                    b.HasOne("StudentManagementAPI.Models.DBModels.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Assignment", b =>
                {
                    b.Navigation("Submissions");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Branch", b =>
                {
                    b.Navigation("CourseOfferings");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Class", b =>
                {
                    b.Navigation("ClassAttendances");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Course", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("CourseOfferings");

                    b.Navigation("Enrollments");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.CourseOffering", b =>
                {
                    b.Navigation("Classes");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Degree", b =>
                {
                    b.Navigation("Branches");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Student", b =>
                {
                    b.Navigation("ClassAttendances");

                    b.Navigation("Enrollments");

                    b.Navigation("Submissions");
                });

            modelBuilder.Entity("StudentManagementAPI.Models.DBModels.Teacher", b =>
                {
                    b.Navigation("CourseOfferings");
                });
#pragma warning restore 612, 618
        }
    }
}