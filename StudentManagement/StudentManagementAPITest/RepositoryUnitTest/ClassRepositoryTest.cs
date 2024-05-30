using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;

namespace StudentManagementAPITest.RepositoryUnitTest
{
    public class ClassRepositoryTest
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
        public async Task TestAddClass()
        {
            //Arrange
            IRepository<int, Class> classRepository = new ClassRepository(context);

            Class classObj = new Class()
            { ClassId = 100, CourseOfferingId = 1, ClassDateAndTime = new DateTime(2024, 5, 6, 9, 0, 0) };

            //Action
            await classRepository.Add(classObj);
            var classResult = await classRepository.Get(100);
            var allClasses = await classRepository.Get();

            //Assert
            Assert.That(classResult.ClassId, Is.EqualTo(100));
            Assert.That(allClasses.Count, Is.EqualTo(11));
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
    }
}
