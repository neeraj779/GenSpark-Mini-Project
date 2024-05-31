using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;

namespace StudentManagementAPITest.RepositoryUnitTest
{
    public class EnrollmentRepositoryTest
    {
        StudentManagementContext context;
        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public async Task TestAddEnrollment()
        {
            //Arrange
            IRepository<int, Enrollment> enrollmentRepository = new EnrollmentRepository(context);
            Enrollment enrollment = new Enrollment()
            { EnrollmentId = 99, StudentId = 4000, CourseCode = "CSE101", EnrollmentDate = new DateTime(2024, 6, 1) };


            //Action
            await enrollmentRepository.Add(enrollment);
            var enrollmentResult = await enrollmentRepository.Get(1);

            //Assert
            Assert.That(enrollmentResult.EnrollmentId, Is.EqualTo(1));
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
    }
}
