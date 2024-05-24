using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Models.DBModels;

public class StudentManagementContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Degree> Degrees { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<ClassAttendance> ClassAttendances { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<CourseOffering> CourseOfferings { get; set; }
    public DbSet<Class> Classes { get; set; }
}
