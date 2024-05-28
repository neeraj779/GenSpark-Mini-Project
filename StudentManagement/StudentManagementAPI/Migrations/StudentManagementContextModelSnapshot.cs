﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace StudentManagementAPI.Migrations
{
    [DbContext(typeof(StudentManagementContext))]
    partial class StudentManagementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("CourseCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int?>("TeacherId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("CourseOfferingId");

                    b.HasIndex("CourseCode");

                    b.HasIndex("TeacherId");

                    b.ToTable("CourseOfferings");
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

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

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
                            Password = new byte[] { 159, 216, 196, 161, 243, 208, 135, 252, 234, 143, 90, 237, 182, 220, 92, 168, 20, 161, 173, 136, 255, 17, 170, 194, 134, 218, 131, 2, 109, 52, 121, 211, 27, 21, 12, 178, 11, 48, 67, 173, 17, 213, 244, 78, 178, 239, 120, 253, 59, 122, 224, 50, 149, 61, 95, 52, 97, 159, 137, 59, 225, 44, 229, 212 },
                            PasswordHashKey = new byte[] { 73, 206, 85, 4, 151, 111, 104, 104, 111, 235, 168, 41, 66, 16, 214, 146, 224, 67, 203, 0, 209, 13, 60, 19, 127, 11, 30, 8, 158, 84, 18, 39, 152, 153, 132, 7, 2, 232, 18, 8, 94, 203, 218, 202, 133, 202, 200, 197, 2, 42, 237, 94, 225, 98, 112, 108, 216, 214, 231, 160, 223, 87, 67, 65, 37, 151, 140, 130, 35, 73, 236, 6, 82, 55, 41, 75, 180, 100, 108, 181, 238, 132, 142, 5, 171, 69, 17, 70, 207, 242, 149, 107, 128, 35, 221, 145, 16, 147, 35, 79, 205, 171, 41, 48, 11, 129, 210, 159, 210, 95, 229, 73, 12, 166, 77, 119, 46, 105, 190, 210, 26, 239, 254, 61, 185, 250, 109, 162 },
                            RegistrationDate = new DateTime(2024, 5, 27, 10, 43, 19, 405, DateTimeKind.Utc).AddTicks(240),
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
                    b.HasOne("StudentManagementAPI.Models.DBModels.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

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
