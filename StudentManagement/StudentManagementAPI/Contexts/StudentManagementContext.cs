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
                FullName = "Mr. Naresh Kumar",
                Email = "kumar.naresh@gmail.com",
                Gender = "Male",
                DateOfBirth = new DateTime(1980, 1, 1),
                Phone = "9876543210"
            }
            );

        modelBuilder.Entity<Student>().HasData(
            new Student
            {
                StudentId = 4000,
                FullName = "Ms. Priya Singh",
                RollNo= "CSE2020001",
                Department = "Computer Science",
                Email = "singh.priya@gmail.com",
                Gender = "Female",
                Phone = "9374729561",
                Status = "Undergraduate",
                DateOfBirth = new DateTime(1999, 1, 1)
            });


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
