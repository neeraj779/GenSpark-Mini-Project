using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;

namespace StudentManagementAPITest.RepositoryUnitTest
{
    public class CourseRepositoryTest
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
        public async Task TestAddCourse()
        {
            //Arrange
            IRepository<string, Course> courseRepository = new CourseRepository(context);
            Course course = new Course()
            {
                CourseCode = "TST101",
                CourseName = "Introduction to Unit Test",
                CourseCredit = 3
            };

            //Action
            await courseRepository.Add(course);
            var courseResult = await courseRepository.Get("TST101");

            //Assert
            Assert.That(courseResult.CourseCode, Is.EqualTo("TST101"));
        }

        [Test]
        public async Task TestGetCourseById()
        {
            //Arrange
            IRepository<string, Course> courseRepository = new CourseRepository(context);

            //Action
            var courseResult = await courseRepository.Get("CSE101");

            //Assert
            Assert.That(courseResult.CourseCode, Is.EqualTo("CSE101"));
        }

        [Test]
        public async Task TestGetAllCourses()
        {
            //Arrange
            IRepository<string, Course> courseRepository = new CourseRepository(context);

            //Action
            var result = await courseRepository.Get();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(11));
        }

        [Test]
        public async Task TestDeleteCourse()
        {
            //Arrange
            IRepository<string, Course> courseRepository = new CourseRepository(context);
            Course course = new Course()
            {
                CourseCode = "TST101",
                CourseName = "Introduction to Unit Test",
                CourseCredit = 3
            };

            //Action
            await courseRepository.Add(course);
            await courseRepository.Delete("TST101");
            var courseResult = await courseRepository.Get("TST101");

            //Assert
            Assert.That(courseResult, Is.Null);
        }

        [Test]
        public async Task TestUpdateCourse()
        {
            //Arrange
            IRepository<string, Course> courseRepository = new CourseRepository(context);
            Course course = new Course()
            {
                CourseCode = "TST101",
                CourseName = "Introduction to Unit Test",
                CourseCredit = 3
            };

            //Action
            await courseRepository.Add(course);
            var courseResult = await courseRepository.Get("TST101");
            courseResult.CourseName = "Updated Course";

            await courseRepository.Update(courseResult);
            var courseRes = await courseRepository.Get("TST101");

            //Assert
            Assert.That(courseRes.CourseName, Is.EqualTo("Updated Course"));
        }
    }
}
