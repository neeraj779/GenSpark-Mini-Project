using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;

namespace StudentManagementAPITest.RepositoryUnitTest
{
    public class CourseOfferingRepositoryTest
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
        public async Task TestAddCourseOffering()
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
            await courseOfferingRepository.Add(courseOffering);
            var courseOfferingResult = await courseOfferingRepository.Get(100);

            //Assert
            Assert.That(courseOfferingResult.CourseOfferingId, Is.EqualTo(100));
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
    }
}
