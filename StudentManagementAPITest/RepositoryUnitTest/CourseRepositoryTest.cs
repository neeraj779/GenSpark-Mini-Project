using Microsoft.EntityFrameworkCore;
using Moq;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;

namespace StudentManagementAPITest.RepositoryUnitTest
{
    public class CourseRepositoryTest
    {
        StudentManagementContext context;
        private Mock<StudentManagementContext> mockContext;
        private CourseRepository mockCourseRepository;


        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            mockContext = new Mock<StudentManagementContext>(optionsBuilder.Options);
            mockCourseRepository = new CourseRepository(mockContext.Object);

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
                CourseCredit = 3,
                CourseOfferings = null,
                Assignments = null,
                Enrollments = null

            };

            //Action
            await courseRepository.Add(course);
            var courseResult = await courseRepository.Get("TST101");

            //Assert
            Assert.That(courseResult.CourseCode, Is.EqualTo("TST101"));
        }

        [Test]
        public void Add_WhenDbUpdateExceptionThrown_ShouldThrowUnableToAddException()
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
            mockContext.Setup(c => c.Add(course)).Verifiable();
            mockContext.Setup(c => c.SaveChangesAsync(default)).ThrowsAsync(new DbUpdateException());
            var ex = Assert.ThrowsAsync<UnableToAddException>(async () => await mockCourseRepository.Add(course));


            //Assert
            Assert.That(ex.Message, Is.EqualTo("Unable to add course. Please check the data and try again."));
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
        public void TestDeleteCourseNotFound()
        {
            //Arrange
            IRepository<string, Course> courseRepository = new CourseRepository(context);

            //Action
            var ex = Assert.ThrowsAsync<NoSuchCourseException>(() => courseRepository.Delete("TST101"));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such course found!"));
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

        [Test]
        public void TestUpdateCourseNotFound()
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
            var ex = Assert.ThrowsAsync<NoSuchCourseException>(() => courseRepository.Update(course));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such course found!"));
        }
    }
}
