using Microsoft.EntityFrameworkCore;
using Moq;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;

namespace StudentManagementAPITest.RepositoryUnitTest
{
    public class ClassAttendanceRepositoryTest
    {
        StudentManagementContext context;
        private Mock<StudentManagementContext> mockContext;
        private ClassAttendanceRepository mockClassAttendanceRepository;



        [SetUp]
        public async Task SetupAsync()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            mockContext = new Mock<StudentManagementContext>(optionsBuilder.Options);
            mockClassAttendanceRepository = new ClassAttendanceRepository(mockContext.Object);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            ClassAttendance classAttendance = new ClassAttendance()
            {
                ClassId = 1,
                AttendanceId = 1,
                StudentId = 4000,
                Status = AttendanceStatus.Present
            };

            IRepository<int, ClassAttendance> classAttendanceRepository = new ClassAttendanceRepository(context);
            await classAttendanceRepository.Add(classAttendance);

        }

        [Test]
        public async Task TestAddClassAttendance()
        {
            //Arrange
            IRepository<int, ClassAttendance> classAttendanceRepository = new ClassAttendanceRepository(context);
            ClassAttendance classAttendance = new ClassAttendance()
            {
                AttendanceId = 2,
                StudentId = 4000,
                ClassId = 1,
                Status = AttendanceStatus.Present,
                Class = null,
                Student = null
            };

            //Action
            await classAttendanceRepository.Add(classAttendance);
            var classAttendanceResult = await classAttendanceRepository.Get(2);

            //Assert
            Assert.That(classAttendanceResult.AttendanceId, Is.EqualTo(2));
        }

        [Test]
        public void Add_WhenDbUpdateExceptionThrown_ShouldThrowUnableToAddException()
        {
            ///Arrange
            IRepository<int, ClassAttendance> classAttendanceRepository = new ClassAttendanceRepository(context);
            ClassAttendance classAttendance = new ClassAttendance()
            {
                AttendanceId = 2,
                StudentId = 4000,
                ClassId = 1,
                Status = AttendanceStatus.Present
            };


            //Action
            mockContext.Setup(c => c.Add(classAttendance)).Verifiable();
            mockContext.Setup(c => c.SaveChangesAsync(default)).ThrowsAsync(new DbUpdateException());

            //Assert
            var exception = Assert.ThrowsAsync<UnableToAddException>(async () => await mockClassAttendanceRepository.Add(classAttendance));
            Assert.That(exception.Message, Is.EqualTo("Unable to add class attendance. Please check the data and try again."));
        }


        [Test]
        public async Task TestGetClassAttendanceById()
        {
            //Arrange
            IRepository<int, ClassAttendance> classAttendanceRepository = new ClassAttendanceRepository(context);

            //Action
            var classAttendanceResult = await classAttendanceRepository.Get(1);

            //Assert
            Assert.That(classAttendanceResult.AttendanceId, Is.EqualTo(1));
        }

        [Test]
        public async Task TestGetAllClassAttendances()
        {
            //Arrange
            IRepository<int, ClassAttendance> classAttendanceRepository = new ClassAttendanceRepository(context);

            //Action
            var classAttendanceResult = await classAttendanceRepository.Get();

            //Assert
            Assert.That(classAttendanceResult.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task TestDeleteClassAttendance()
        {
            //Arrange
            IRepository<int, ClassAttendance> classAttendanceRepository = new ClassAttendanceRepository(context);

            //Action
            await classAttendanceRepository.Delete(1);
            var classAttendanceResult = await classAttendanceRepository.Get(1);

            //Assert
            Assert.That(classAttendanceResult, Is.Null);
        }

        [Test]
        public void TestDeleteClassAttendanceFail()
        {
            //Arrange
            IRepository<int, ClassAttendance> classAttendanceRepository = new ClassAttendanceRepository(context);

            //Action
            var ex = Assert.ThrowsAsync<NoSuchClassAttendanceException>(() => classAttendanceRepository.Delete(2));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such class attendance found."));
        }

        [Test]
        public async Task TestUpdateClassAttendance()
        {
            //Arrange
            IRepository<int, ClassAttendance> classAttendanceRepository = new ClassAttendanceRepository(context);

            //Action
            var classAttendance = await classAttendanceRepository.Get(1);
            classAttendance.Status = AttendanceStatus.Absent;

            await classAttendanceRepository.Update(classAttendance);

            var updatedClassAttendance = await classAttendanceRepository.Get(1);

            //Assert
            Assert.That(updatedClassAttendance.Status, Is.EqualTo(AttendanceStatus.Absent));
        }

        [Test]
        public void TestUpdateClassAttendanceFail()
        {
            //Arrange
            IRepository<int, ClassAttendance> classAttendanceRepository = new ClassAttendanceRepository(context);

            ClassAttendance classAttendance = new ClassAttendance()
            {
                AttendanceId = 2,
                StudentId = 4000,
                ClassId = 1,
                Status = AttendanceStatus.Present
            };

            //Action
            var ex = Assert.ThrowsAsync<NoSuchClassAttendanceException>(() => classAttendanceRepository.Update(classAttendance));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such class attendance found."));
        }
    }
}
