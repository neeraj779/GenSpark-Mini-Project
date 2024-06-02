using Microsoft.EntityFrameworkCore;
using Moq;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;

namespace StudentManagementAPITest.RepositoryUnitTest
{
    public class CourseOfferingRepositoryTest
    {
        StudentManagementContext context;
        private Mock<StudentManagementContext> mockContext;
        private CourseOfferingRepository mockCourseOfferingRepository;


        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            mockContext = new Mock<StudentManagementContext>(optionsBuilder.Options);
            mockCourseOfferingRepository = new CourseOfferingRepository(mockContext.Object);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public async Task TestAddCourseOffering()
        {
            //Arrange
            IRepository<int, CourseOffering> courseOfferingRepository = new CourseOfferingRepository(context);
            CourseOffering courseOffering = new CourseOffering()
            {
                CourseOfferingId = 100,
                CourseCode = "CSE101",
                TeacherId = 2000,
                Course = null,
                Teacher = null,
                Classes = null
            };

            //Action
            await courseOfferingRepository.Add(courseOffering);
            var courseOfferingResult = await courseOfferingRepository.Get(100);

            //Assert
            Assert.That(courseOfferingResult.CourseOfferingId, Is.EqualTo(100));
        }

        [Test]
        public void Add_WhenDbUpdateExceptionThrown_ShouldThrowUnableToAddException()
        {
            //Arrange
            IRepository<int, CourseOffering> courseOfferingRepository = new CourseOfferingRepository(context);
            CourseOffering courseOffering = new CourseOffering()
            {
                CourseOfferingId = 100,
                CourseCode = "CSE101",
                TeacherId = 2000,
            };

            //Action
            mockContext.Setup(c => c.Add(courseOffering)).Verifiable();
            mockContext.Setup(c => c.SaveChangesAsync(default)).ThrowsAsync(new DbUpdateException());

            //Assert
            var exception = Assert.ThrowsAsync<UnableToAddException>(async () => await mockCourseOfferingRepository.Add(courseOffering));
            Assert.That(exception.Message, Is.EqualTo("Unable to add course offering. Please check the data and try again."));
        }

        [Test]
        public async Task TestGetCourseOfferingById()
        {
            //Arrange
            IRepository<int, CourseOffering> courseOfferingRepository = new CourseOfferingRepository(context);

            //Action
            var courseOfferingResult = await courseOfferingRepository.Get(1);

            //Assert
            Assert.That(courseOfferingResult.CourseOfferingId, Is.EqualTo(1));
        }

        [Test]
        public async Task TestGetAllCourseOfferings()
        {
            //Arrange
            IRepository<int, CourseOffering> courseOfferingRepository = new CourseOfferingRepository(context);

            //Action
            var courseOfferingResult = await courseOfferingRepository.Get();

            //Assert
            Assert.That(courseOfferingResult.Count, Is.EqualTo(5));
        }

        [Test]
        public async Task TestDeleteCourseOffering()
        {
            //Arrange
            IRepository<int, CourseOffering> courseOfferingRepository = new CourseOfferingRepository(context);

            //Action
            await courseOfferingRepository.Delete(1);
            var courseOfferingResult = await courseOfferingRepository.Get(1);

            //Assert
            Assert.That(courseOfferingResult, Is.Null);
        }

        [Test]
        public void TestDeleteCourseOfferingException()
        {
            //Arrange
            IRepository<int, CourseOffering> courseOfferingRepository = new CourseOfferingRepository(context);

            //Action
            var ex = Assert.ThrowsAsync<NoSuchCourseOfferingException>(() => courseOfferingRepository.Delete(100));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such course offering found."));
        }

        [Test]
        public async Task TestUpdateCourseOffering()
        {
            //Arrange
            IRepository<int, CourseOffering> courseOfferingRepository = new CourseOfferingRepository(context);

            //Action
            var courseOffering = await courseOfferingRepository.Get(1);
            courseOffering.TeacherId = 2001;

            await courseOfferingRepository.Update(courseOffering);
            var courseOfferingResult = await courseOfferingRepository.Get(1);

            //Assert
            Assert.That(courseOfferingResult.TeacherId, Is.EqualTo(2001));
        }

        [Test]
        public void TestUpdateCourseOfferingException()
        {
            //Arrange
            IRepository<int, CourseOffering> courseOfferingRepository = new CourseOfferingRepository(context);
            CourseOffering courseOffering = new CourseOffering()
            {
                CourseOfferingId = 200,
                CourseCode = "CSE101",
                TeacherId = 2000,
            };

            //Action
            var ex = Assert.ThrowsAsync<NoSuchCourseOfferingException>(() => courseOfferingRepository.Update(courseOffering));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such course offering found."));
        }
    }
}
