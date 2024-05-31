using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;
using StudentManagementAPI.Repositories;
using StudentManagementAPI.Services;

namespace StudentManagementAPITest.ServiceUnitTest
{
    public class CourseServiceTest
    {
        StudentManagementContext context;
        IRepository<string, Course> couserRepository;
        CourseService courseService;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            couserRepository = new CourseRepository(context);
            courseService = new CourseService(couserRepository);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public async Task CreateCourseTest()
        {
            // Arrange
            CourseDTO course = new CourseDTO()
            {
                CourseCode = "SEC101",
                CourseName = "DSP",
                CourseCredit = 3
            };

            // Action
            var createdCourse = await courseService.CreateCourse(course);

            // Assert
            Assert.That(createdCourse.CourseCode, Is.EqualTo(course.CourseCode));
            Assert.That(createdCourse.CourseName, Is.EqualTo(course.CourseName));
            Assert.That(createdCourse.CourseCredit, Is.EqualTo(course.CourseCredit));
        }

        [Test]
        public void CreateExistingCourseTest()
        {
            // Arrange
            CourseDTO course = new CourseDTO()
            {
                CourseCode = "CSE101",
                CourseName = "Introduction to Computer Science",
                CourseCredit = 3
            };

            // Assert
            var ex = Assert.ThrowsAsync<CourseAlreadyExistsException>(() => courseService.CreateCourse(course));

            Assert.That(ex.Message, Is.EqualTo("Course already exists."));
        }

        [Test]
        public async Task DeleteCourseTest()
        {
            // Arrange
            string courseCode = "CSE101";

            // Action
            var deletedCourse = await courseService.DeleteCourse(courseCode);
            var courseCount = await couserRepository.Get();

            // Assert
            Assert.That(deletedCourse.CourseCode, Is.EqualTo(courseCode));
            Assert.That(courseCount.Count(), Is.EqualTo(10));
        }

        [Test]
        public void DeleteNonExistingCourseTest()
        {
            // Arrange
            string courseCode = "INVALID";

            // Assert
            var ex = Assert.ThrowsAsync<NoSuchCourseException>(() => courseService.DeleteCourse(courseCode));

            Assert.That(ex.Message, Is.EqualTo("No such course found!"));
        }

        [Test]
        public async Task GetCourseByCodeTest()
        {
            // Arrange
            string courseCode = "CSE101";

            // Action
            var course = await courseService.GetCourseByCode(courseCode);

            // Assert
            Assert.That(course.CourseCode, Is.EqualTo(courseCode));
        }

        [Test]
        public void GetNonExistingCourseByCodeTest()
        {
            // Arrange
            string courseCode = "INVALID";

            // Assert
            var ex = Assert.ThrowsAsync<NoSuchCourseException>(() => courseService.GetCourseByCode(courseCode));

            Assert.That(ex.Message, Is.EqualTo("No such course found!"));
        }

        [Test]
        public async Task GetCoursesTest()
        {
            // Action
            var courses = await courseService.GetCourses();

            // Assert
            Assert.That(courses.Count(), Is.EqualTo(11));
        }

        [Test]
        public void GetCoursesNoCourseFoundTest()
        {
            // Arrange
            context.Courses.RemoveRange(context.Courses);
            context.SaveChanges();

            // Assert
            var ex = Assert.ThrowsAsync<NoCourseFoundException>(() => courseService.GetCourses());

            Assert.That(ex.Message, Is.EqualTo("No course found!"));
        }

        [Test]
        public async Task UpdateCourseCreditHoursTest()
        {
            // Arrange
            string courseCode = "CSE101";
            int creditHours = 4;

            // Action
            var updatedCourse = await courseService.UpdateCourseCreditHours(courseCode, creditHours);

            // Assert
            Assert.That(updatedCourse.CourseCode, Is.EqualTo(courseCode));
            Assert.That(updatedCourse.CourseCredit, Is.EqualTo(creditHours));
        }

        [Test]
        public void UpdateNonExistingCourseCreditHoursTest()
        {
            // Arrange
            string courseCode = "INVALID";
            int creditHours = 4;

            // Assert
            var ex = Assert.ThrowsAsync<NoSuchCourseException>(() => courseService.UpdateCourseCreditHours(courseCode, creditHours));

            Assert.That(ex.Message, Is.EqualTo("No such course found!"));
        }
    }
}
