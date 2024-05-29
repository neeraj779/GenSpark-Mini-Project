using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Models.DBModels;
using System.Security.Cryptography;
using System.Text;

public class StudentManagementContext : DbContext
{
    public StudentManagementContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<ClassAttendance> ClassAttendances { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<CourseOffering> CourseOfferings { get; set; }
    public DbSet<Class> Classes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var adminUser = new User
        {
            UserId = 100,
            UserName = "admin",
            Status = "Active",
            Role = UserRole.Admin,
            RegistrationDate = DateTime.UtcNow
        };

        HMACSHA512 hMACSHA = new HMACSHA512();
        adminUser.PasswordHashKey = hMACSHA.Key;
        adminUser.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes("admin"));


        modelBuilder.Entity<User>().HasData(adminUser);

        modelBuilder.Entity<Course>().HasData(
            new Course { CourseCode = "CSE101", CourseName = "Introduction to Computer Science", CourseCredit = 3 },
            new Course { CourseCode = "CSE102", CourseName = "Data Structures", CourseCredit = 4 },
            new Course { CourseCode = "CSE103", CourseName = "Algorithms", CourseCredit = 2 },
            new Course { CourseCode = "CSE104", CourseName = "Database Management Systems", CourseCredit = 3 },
            new Course { CourseCode = "CSE105", CourseName = "Operating Systems", CourseCredit = 2 },
            new Course { CourseCode = "CSE106", CourseName = "Computer Networks", CourseCredit = 2 },
            new Course { CourseCode = "CSE107", CourseName = "Software Engineering", CourseCredit = 4 },
            new Course { CourseCode = "CSE108", CourseName = "Web Development", CourseCredit = 3 },
            new Course { CourseCode = "CSE109", CourseName = "Artificial Intelligence", CourseCredit = 4 },
            new Course { CourseCode = "CSE110", CourseName = "Machine Learning", CourseCredit = 3 },
            new Course { CourseCode = "CSE201", CourseName = "Object-Oriented Programming", CourseCredit = 3 }
        );

        modelBuilder.Entity<Teacher>().HasData(
            new Teacher
            {
                TeacherId = 2000,
                FullName = "Dr. Sunita Verma",
                Email = "sunita.verma@gmail.com",
                Gender = "Female",
                DateOfBirth = new DateTime(1978, 3, 15),
                Phone = "9876543212"
            },
            new Teacher
            {
                TeacherId = 2002,
                FullName = "Mr. Ramesh Gupta",
                Email = "ramesh.gupta@gmail.com",
                Gender = "Male",
                DateOfBirth = new DateTime(1975, 6, 25),
                Phone = "9876543213"
            },
            new Teacher
            {
                TeacherId = 2003,
                FullName = "Ms. Anjali Mehta",
                Email = "anjali.mehta@gmail.com",
                Gender = "Female",
                DateOfBirth = new DateTime(1982, 11, 30),
                Phone = "9876543214"
            },
            new Teacher
            {
                TeacherId = 2004,
                FullName = "Mr. Vijay Patil",
                Email = "vijay.patil@gmail.com",
                Gender = "Male",
                DateOfBirth = new DateTime(1970, 9, 10),
                Phone = "9876543215"
            },
            new Teacher
            {
                TeacherId = 2005,
                FullName = "Dr. Seema Reddy",
                Email = "seema.reddy@gmail.com",
                Gender = "Female",
                DateOfBirth = new DateTime(1983, 5, 20),
                Phone = "9876543216"
            });

        modelBuilder.Entity<Student>().HasData(
            new Student
            {
                StudentId = 4001,
                FullName = "Mr. Raj Patel",
                RollNo = "CSE2020002",
                Department = "Computer Science",
                Email = "raj.patel@gmail.com",
                Gender = "Male",
                Phone = "9374729562",
                Status = StudentStatus.Undergraduate,
                DateOfBirth = new DateTime(1998, 2, 15)
            },
            new Student
            {
                StudentId = 4002,
                FullName = "Ms. Neha Desai",
                RollNo = "ECE2020001",
                Department = "Electronics and Communication",
                Email = "neha.desai@gmail.com",
                Gender = "Female",
                Phone = "9374729563",
                Status = StudentStatus.Undergraduate,
                DateOfBirth = new DateTime(2000, 5, 20)
            },
            new Student
            {
                StudentId = 4003,
                FullName = "Mr. Amit Sharma",
                RollNo = "ME2020001",
                Department = "Mechanical Engineering",
                Email = "amit.sharma@gmail.com",
                Gender = "Male",
                Phone = "9374729564",
                Status = StudentStatus.Undergraduate,
                DateOfBirth = new DateTime(1997, 8, 25)
            },
            new Student
            {
                StudentId = 4004,
                FullName = "Ms. Sneha Rao",
                RollNo = "EE2020001",
                Department = "Electrical Engineering",
                Email = "sneha.rao@gmail.com",
                Gender = "Female",
                Phone = "9374729565",
                Status = StudentStatus.Undergraduate,
                DateOfBirth = new DateTime(1999, 3, 10)
            },
            new Student
            {
                StudentId = 4005,
                FullName = "Mr. Vivek Gupta",
                RollNo = "CE2020001",
                Department = "Civil Engineering",
                Email = "vivek.gupta@gmail.com",
                Gender = "Male",
                Phone = "9374729566",
                Status = StudentStatus.Undergraduate,
                DateOfBirth = new DateTime(1996, 11, 30)
            });


        modelBuilder.Entity<CourseOffering>().HasData(
            new CourseOffering { CourseOfferingId = 1, CourseCode = "CSE101", TeacherId = 2000 },
            new CourseOffering { CourseOfferingId = 2, CourseCode = "CSE102", TeacherId = 2002 },
            new CourseOffering { CourseOfferingId = 3, CourseCode = "CSE103", TeacherId = 2003 },
            new CourseOffering { CourseOfferingId = 4, CourseCode = "CSE104", TeacherId = 2004 },
            new CourseOffering { CourseOfferingId = 5, CourseCode = "CSE105", TeacherId = 2005 }
        );

        modelBuilder.Entity<Class>().HasData(
            new Class { ClassId = 1, CourseOfferingId = 1, ClassDateAndTime = new DateTime(2024, 5, 6, 9, 0, 0) },
            new Class { ClassId = 2, CourseOfferingId = 1, ClassDateAndTime = new DateTime(2024, 5, 8, 9, 0, 0) },
            new Class { ClassId = 3, CourseOfferingId = 1, ClassDateAndTime = new DateTime(2024, 5, 10, 9, 0, 0) },
            new Class { ClassId = 4, CourseOfferingId = 1, ClassDateAndTime = new DateTime(2024, 5, 12, 9, 0, 0) },
            new Class { ClassId = 5, CourseOfferingId = 1, ClassDateAndTime = new DateTime(2024, 5, 14, 9, 0, 0) },
            new Class { ClassId = 6, CourseOfferingId = 2, ClassDateAndTime = new DateTime(2024, 5, 7, 2, 0, 0) },
            new Class { ClassId = 7, CourseOfferingId = 2, ClassDateAndTime = new DateTime(2024, 5, 9, 11, 0, 0) },
            new Class { ClassId = 8, CourseOfferingId = 2, ClassDateAndTime = new DateTime(2024, 5, 11, 13, 0, 0) },
            new Class { ClassId = 9, CourseOfferingId = 2, ClassDateAndTime = new DateTime(2024, 5, 13, 14, 0, 0) },
            new Class { ClassId = 10, CourseOfferingId = 2, ClassDateAndTime = new DateTime(2024, 5, 15, 9, 0, 0) }
        );

        modelBuilder.Entity<Enrollment>().HasData(
            new Enrollment { EnrollmentId = 1, StudentId = 4001, CourseCode = "CSE101", EnrollmentDate = new DateTime(2024, 6, 1) },
            new Enrollment { EnrollmentId = 2, StudentId = 4001, CourseCode = "CSE102", EnrollmentDate = new DateTime(2024, 6, 1) },
            new Enrollment { EnrollmentId = 3, StudentId = 4001, CourseCode = "CSE103", EnrollmentDate = new DateTime(2024, 6, 1) },
            new Enrollment { EnrollmentId = 4, StudentId = 4002, CourseCode = "CSE101", EnrollmentDate = new DateTime(2024, 6, 1) },
            new Enrollment { EnrollmentId = 5, StudentId = 4002, CourseCode = "CSE102", EnrollmentDate = new DateTime(2024, 6, 1) },
            new Enrollment { EnrollmentId = 6, StudentId = 4002, CourseCode = "CSE103", EnrollmentDate = new DateTime(2024, 6, 1) },
            new Enrollment { EnrollmentId = 7, StudentId = 4003, CourseCode = "CSE101", EnrollmentDate = new DateTime(2024, 6, 1) },
            new Enrollment { EnrollmentId = 8, StudentId = 4003, CourseCode = "CSE102", EnrollmentDate = new DateTime(2024, 6, 1) },
            new Enrollment { EnrollmentId = 9, StudentId = 4003, CourseCode = "CSE103", EnrollmentDate = new DateTime(2024, 6, 1) },
            new Enrollment { EnrollmentId = 10, StudentId = 4004, CourseCode = "CSE101", EnrollmentDate = new DateTime(2024, 6, 1) },
            new Enrollment { EnrollmentId = 11, StudentId = 4004, CourseCode = "CSE102", EnrollmentDate = new DateTime(2024, 6, 1) },
            new Enrollment { EnrollmentId = 12, StudentId = 4004, CourseCode = "CSE103", EnrollmentDate = new DateTime(2024, 6, 1) },
            new Enrollment { EnrollmentId = 13, StudentId = 4005, CourseCode = "CSE101", EnrollmentDate = new DateTime(2024, 6, 1) },
            new Enrollment { EnrollmentId = 14, StudentId = 4005, CourseCode = "CSE102", EnrollmentDate = new DateTime(2024, 6, 1) },
            new Enrollment { EnrollmentId = 15, StudentId = 4005, CourseCode = "CSE103", EnrollmentDate = new DateTime(2024, 6, 1) }
        );

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseCode)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClassAttendance>()
                .HasOne(ca => ca.Class)
                .WithMany(c => c.ClassAttendances)
                .HasForeignKey(ca => ca.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClassAttendance>()
                .HasOne(ca => ca.Student)
                .WithMany(s => s.ClassAttendances)
                .HasForeignKey(ca => ca.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Course)
                .WithMany(c => c.Assignments)
                .HasForeignKey(a => a.CourseCode)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Submission>()
                .HasOne(s => s.Assignment)
                .WithMany(a => a.Submissions)
                .HasForeignKey(s => s.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Submission>()
                .HasOne(s => s.Student)
                .WithMany(s => s.Submissions)
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Teacher>()
                .HasMany(t => t.CourseOfferings)
                .WithOne(co => co.Teacher)
                .HasForeignKey(co => co.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}
