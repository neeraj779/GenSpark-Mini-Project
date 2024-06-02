using Microsoft.EntityFrameworkCore;
using Moq;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;

namespace StudentManagementAPITest.RepositoryUnitTest
{
    public class ClassRepositoryTest
    {
        StudentManagementContext context;
        private Mock<StudentManagementContext> mockContext;
        private ClassRepository mockClassRepository;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            mockContext = new Mock<StudentManagementContext>(optionsBuilder.Options);
            mockClassRepository = new ClassRepository(mockContext.Object);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public async Task TestAddClass()
        {
            //Arrange
            IRepository<int, Class> classRepository = new ClassRepository(context);

            Class classObj = new Class()
            { ClassId = 100, CourseOfferingId = 1, ClassDateAndTime = new DateTime(2024, 5, 6, 9, 0, 0), CourseOffering = null, ClassAttendances = null };

            //Action
            await classRepository.Add(classObj);
            var classResult = await classRepository.Get(100);
            var allClasses = await classRepository.Get();

            //Assert
            Assert.That(classResult.ClassId, Is.EqualTo(100));
            Assert.That(allClasses.Count, Is.EqualTo(11));
        }

        [Test]
        public void Add_WhenDbUpdateExceptionThrown_ShouldThrowUnableToAddException()
        {
            //Arrange
            IRepository<int, Class> classRepository = new ClassRepository(context);

            Class classObj = new Class()
            { ClassId = 100, CourseOfferingId = 1, ClassDateAndTime = new DateTime(2024, 5, 6, 9, 0, 0) };

            //Action
            mockContext.Setup(c => c.Add(classObj)).Verifiable();
            mockContext.Setup(c => c.SaveChangesAsync(default)).ThrowsAsync(new DbUpdateException());

            //Assert
            var exception = Assert.ThrowsAsync<UnableToAddException>(async () => await mockClassRepository.Add(classObj));
            Assert.That(exception.Message, Is.EqualTo("Unable to add class. Please check the data and try again."));
        }

        [Test]
        public async Task TestGetClassById()
        {
            //Arrange
            IRepository<int, Class> classRepository = new ClassRepository(context);

            //Action
            var classResult = await classRepository.Get(1);

            //Assert
            Assert.That(classResult.ClassId, Is.EqualTo(1));
        }

        [Test]
        public async Task TestGetAllClasses()
        {
            //Arrange
            IRepository<int, Class> classRepository = new ClassRepository(context);

            //Action
            var allClasses = await classRepository.Get();

            //Assert
            Assert.That(allClasses.Count, Is.EqualTo(10));
        }

        [Test]
        public async Task TestDeleteClass()
        {
            //Arrange
            IRepository<int, Class> classRepository = new ClassRepository(context);

            //Action
            await classRepository.Delete(1);
            var allClasses = await classRepository.Get();

            //Assert
            Assert.That(allClasses.Count, Is.EqualTo(9));
        }

        [Test]
        public void TestDeleteClassNotFound()
        {
            //Arrange
            IRepository<int, Class> classRepository = new ClassRepository(context);

            //Action
            var ex = Assert.ThrowsAsync<NoSuchClassException>(async () => await classRepository.Delete(100));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Class with given Id does not exist."));
        }

        [Test]
        public async Task TestUpdateClass()
        {
            //Arrange
            IRepository<int, Class> classRepository = new ClassRepository(context);

            //Action
            var classObj = await classRepository.Get(1);
            classObj.ClassDateAndTime = new DateTime(2024, 6, 1, 9, 0, 0);

            await classRepository.Update(classObj);
            var classResult = await classRepository.Get(1);

            //Assert
            Assert.That(classResult.ClassDateAndTime, Is.EqualTo(new DateTime(2024, 6, 1, 9, 0, 0)));
        }

        [Test]
        public void TestUpdateClassNotFound()
        {
            //Arrange
            IRepository<int, Class> classRepository = new ClassRepository(context);
            Class classObj = new Class()
            { ClassId = 100, CourseOfferingId = 2, ClassDateAndTime = new DateTime(2024, 6, 1, 9, 0, 0) };

            //Action
            var ex = Assert.ThrowsAsync<NoSuchClassException>(async () => await classRepository.Update(classObj));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Class with given Id does not exist."));
        }
    }
}
