using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;
using StudentManagementAPI.Services;

namespace StudentManagementAPITest.ServiceUnitTest
{
    public class EnrollmentServiceTest
    {
        StudentManagementContext context;
        IRepository<int, Enrollment> enrollmentRepository;
        IRepository<int, Student> studentRepository;
        IRepository<string, Course> courseRepository;
        EnrollmentService enrollmentService;
        Mock<ILogger<EnrollmentService>> loggerMock;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<StudentManagementContext>()
                .UseInMemoryDatabase("StudentManagementDB")
                .Options;
            context = new StudentManagementContext(options);

            enrollmentRepository = new EnrollmentRepository(context);
            studentRepository = new StudentRepository(context);
            courseRepository = new CourseRepository(context);

            loggerMock = new Mock<ILogger<EnrollmentService>>();

            enrollmentService = new EnrollmentService(enrollmentRepository, studentRepository, courseRepository, loggerMock.Object);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public async Task EnrollStudent_ValidStudentAndCourse_EnrollsStudent()
        {
            // Arrange
            int studentId = 4000;
            string courseCode = "CSE201";

            // Act
            var result = await enrollmentService.EnrollStudent(studentId, courseCode);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StudentId, Is.EqualTo(studentId));
            Assert.That(result.CourseCode, Is.EqualTo(courseCode));
        }


        [Test]
        public void EnrollStudent_StudentDoesNotExist_ThrowsNoSuchStudentException()
        {
            // Arrange
            int studentId = 9999;
            string courseCode = "CS101";

            // Action
            var ex = Assert.ThrowsAsync<NoSuchStudentException>(() => enrollmentService.EnrollStudent(studentId, courseCode));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such student found!"));
        }

        [Test]
        public void EnrollStudent_CourseDoesNotExist_ThrowsNoSuchCourseException()
        {
            // Arrange
            int studentId = 4000;
            string courseCode = "INVALID";

            // Action
            var ex = Assert.ThrowsAsync<NoSuchCourseException>(() => enrollmentService.EnrollStudent(studentId, courseCode));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No such course found!"));
        }

        [Test]
        public void EnrollStudent_StudentAlreadyEnrolled_ThrowsStudentAlreadyEnrolledException()
        {
            // Arrange
            int studentId = 4000;
            string courseCode = "CSE101";
            context.Enrollments.Add(new Enrollment { StudentId = studentId, CourseCode = courseCode });
            context.SaveChanges();

            // Action
            var ex = Assert.ThrowsAsync<StudentAlreadyEnrolledException>(() => enrollmentService.EnrollStudent(studentId, courseCode));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Student is already enrolled in the course!"));
        }


        [Test]
        public async Task UnenrollStudent_ValidEnrollment_UnenrollsStudent()
        {
            // Arrange
            int studentId = 4000;
            string courseCode = "CSE101";
            var enrollment = new Enrollment { StudentId = studentId, CourseCode = courseCode };
            context.Enrollments.Add(enrollment);
            context.SaveChanges();

            // Act
            var result = await enrollmentService.UnenrollStudent(studentId, courseCode);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StudentId, Is.EqualTo(studentId));
            Assert.That(result.CourseCode, Is.EqualTo(courseCode));
        }

        [Test]
        public void UnenrollStudent_EnrollmentDoesNotExist_ThrowsNoSuchEnrollmentException()
        {
            // Arrange
            int studentId = 4000;
            string courseCode = "CSE110";

            // Action
            var ex = Assert.ThrowsAsync<NoSuchEnrollmentException>(() => enrollmentService.UnenrollStudent(studentId, courseCode));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No such enrollment found!"));
        }

        [Test]
        public void UnenrollStudent_StudentDoesNotExist_ThrowsNoSuchStudentException()
        {
            // Arrange
            int studentId = 9999;
            string courseCode = "CSE101";

            // Action
            var ex = Assert.ThrowsAsync<NoSuchStudentException>(() => enrollmentService.UnenrollStudent(studentId, courseCode));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such student found!"));
        }

        [Test]
        public void UnenrollStudent_CourseDoesNotExist_ThrowsNoSuchCourseException()
        {
            // Arrange
            int studentId = 4000;
            string courseCode = "INVALID";

            // Action
            var ex = Assert.ThrowsAsync<NoSuchCourseException>(() => enrollmentService.UnenrollStudent(studentId, courseCode));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No such course found!"));
        }

        [Test]
        public async Task GetEnrollmentsByStudentId_ValidStudent_ReturnsEnrollments()
        {
            // Arrange
            int studentId = 4000;

            // Act
            var result = await enrollmentService.GetEnrollmentsByStudentId(studentId);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetEnrollmentsByStudentId_NoEnrollments_ThrowsNoSuchEnrollmentException()
        {
            // Arrange
            context.Enrollments.RemoveRange(context.Enrollments);
            context.SaveChanges();
            int studentId = 4004;

            // Action
            var ex = Assert.ThrowsAsync<NoSuchEnrollmentException>(() => enrollmentService.GetEnrollmentsByStudentId(studentId));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such enrollment found!"));
        }

        [Test]
        public async Task GetEnrollmentsByCourseId_ValidCourse_ReturnsEnrollments()
        {
            // Arrange
            string CourseCode = "CSE101";

            // Act
            var result = await enrollmentService.GetEnrollmentsByCourseCode(CourseCode);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetEnrollmentsByCourseId_NoEnrollments_ThrowsNoSuchEnrollmentException()
        {
            // Arrange
            string courseCode = "CSE201";

            // Action
            var ex = Assert.ThrowsAsync<NoSuchEnrollmentException>(() => enrollmentService.GetEnrollmentsByCourseCode(courseCode));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No such enrollment found!"));
        }

        [Test]
        public async Task GetAllEnrollments_ReturnsEnrollments()
        {
            // Act
            var result = await enrollmentService.GetAllEnrollments();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(15));
        }

        [Test]
        public void GetAllEnrollments_NoEnrollments_ThrowsNoEnrollmentFoundException()
        {
            // Arrange
            context.Enrollments.RemoveRange(context.Enrollments);
            context.SaveChanges();

            // Action
            var ex = Assert.ThrowsAsync<NoEnrollmentFoundException>(() => enrollmentService.GetAllEnrollments());

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No enrollment found!"));
        }
    }
}
