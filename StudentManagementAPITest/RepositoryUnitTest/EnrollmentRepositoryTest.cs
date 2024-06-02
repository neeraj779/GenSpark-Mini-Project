using Microsoft.EntityFrameworkCore;
using Moq;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;

namespace StudentManagementAPITest.RepositoryUnitTest
{
    public class EnrollmentRepositoryTest
    {
        StudentManagementContext context;
        private Mock<StudentManagementContext> mockContext;
        private EnrollmentRepository mockEnrollmentRepository;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            mockContext = new Mock<StudentManagementContext>(optionsBuilder.Options);
            mockEnrollmentRepository = new EnrollmentRepository(mockContext.Object);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public async Task TestAddEnrollment()
        {
            //Arrange
            IRepository<int, Enrollment> enrollmentRepository = new EnrollmentRepository(context);
            Enrollment enrollment = new Enrollment()
            { EnrollmentId = 99, StudentId = 4000, CourseCode = "CSE101", EnrollmentDate = new DateTime(2024, 6, 1), Student = null, Course = null };


            //Action
            await enrollmentRepository.Add(enrollment);
            var enrollmentResult = await enrollmentRepository.Get(1);

            //Assert
            Assert.That(enrollmentResult.EnrollmentId, Is.EqualTo(1));
        }

        [Test]
        public void Add_WhenDbUpdateExceptionThrown_ShouldThrowUnableToAddException()
        {
            //Arrange
            IRepository<int, Enrollment> enrollmentRepository = new EnrollmentRepository(context);
            Enrollment enrollment = new Enrollment()
            { EnrollmentId = 99, StudentId = 4000, CourseCode = "CSE101", EnrollmentDate = new DateTime(2024, 6, 1) };

            //Action
            mockContext.Setup(c => c.Add(enrollment)).Verifiable();
            mockContext.Setup(c => c.SaveChangesAsync(default)).ThrowsAsync(new DbUpdateException());

            //Assert
            var exception = Assert.ThrowsAsync<UnableToAddException>(async () => await mockEnrollmentRepository.Add(enrollment));
            Assert.That(exception.Message, Is.EqualTo("Unable to add Enrollment. Please check the data and try again."));
        }


        [Test]
        public async Task TestGetEnrollmentById()
        {
            //Arrange
            IRepository<int, Enrollment> enrollmentRepository = new EnrollmentRepository(context);

            //Action
            var enrollmentResult = await enrollmentRepository.Get(1);

            //Assert
            Assert.That(enrollmentResult.EnrollmentId, Is.EqualTo(1));
        }

        [Test]
        public async Task TestGetAllEnrollments()
        {
            //Arrange
            IRepository<int, Enrollment> enrollmentRepository = new EnrollmentRepository(context);

            //Action
            var result = await enrollmentRepository.Get();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(15));
        }

        [Test]
        public async Task TestDeleteEnrollment()
        {
            //Arrange
            IRepository<int, Enrollment> enrollmentRepository = new EnrollmentRepository(context);

            //Action
            var result = await enrollmentRepository.Delete(1);
            var resultAll = await enrollmentRepository.Get();

            //Assert
            Assert.That(resultAll.Count(), Is.EqualTo(14));
            Assert.That(result.EnrollmentId, Is.EqualTo(1));
        }

        [Test]
        public void Delete_WhenNoSuchEnrollmentExceptionThrown_ShouldThrowNoSuchEnrollmentException()
        {
            //Arrange
            IRepository<int, Enrollment> enrollmentRepository = new EnrollmentRepository(context);

            //Action
            var ex = Assert.ThrowsAsync<NoSuchEnrollmentException>(async () => await enrollmentRepository.Delete(100));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such enrollment found!"));
        }

        [Test]
        public async Task TestUpdateEnrollment()
        {
            //Arrange
            IRepository<int, Enrollment> enrollmentRepository = new EnrollmentRepository(context);

            //Action
            var enrollment = await enrollmentRepository.Get(1);
            enrollment.CourseCode = "CSE102";

            await enrollmentRepository.Update(enrollment);
            var result = await enrollmentRepository.Get(1);

            //Assert
            Assert.That(result.CourseCode, Is.EqualTo("CSE102"));
        }

        [Test]
        public void Update_WhenNoSuchEnrollmentExceptionThrown_ShouldThrowNoSuchEnrollmentException()
        {
            //Arrange
            IRepository<int, Enrollment> enrollmentRepository = new EnrollmentRepository(context);

            //Action
            Enrollment enrollment = new Enrollment()
            { EnrollmentId = 100, StudentId = 4000, CourseCode = "CSE101", EnrollmentDate = new DateTime(2024, 6, 1) };

            //Assert
            var ex = Assert.ThrowsAsync<NoSuchEnrollmentException>(async () => await enrollmentRepository.Update(enrollment));
            Assert.That(ex.Message, Is.EqualTo("No such enrollment found!"));
        }
    }
}
