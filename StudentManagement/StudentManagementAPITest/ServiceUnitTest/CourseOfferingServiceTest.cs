using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;
using StudentManagementAPI.Services;

namespace StudentManagementAPITest.ServiceUnitTest
{
    public class CourseOfferingServiceTest
    {
        StudentManagementContext context;
        IRepository<int, CourseOffering> courseOfferingRepository;
        IRepository<string, Course> courseRepository;
        IRepository<int, Teacher> teacherRepository;

        CourseOfferingService courseOfferingService;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            courseOfferingRepository = new CourseOfferingRepository(context);
            courseRepository = new CourseRepository(context);
            teacherRepository = new TeacherRepository(context);

            courseOfferingService = new CourseOfferingService(courseOfferingRepository, courseRepository, teacherRepository);


            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public async Task AssignTeacherForCourseOffering_WhenCourseAndTeacherExists_ShouldReturnCourseOfferingDTO()
        {
            //Arrange
            int teacherId = 2000;
            string courseCode = "CSE201";


            //Action
            var result = await courseOfferingService.AssignTeacherForCourseOffering(teacherId, courseCode);

            //Assert
            Assert.NotNull(result);
            Assert.That(result.CourseCode, Is.EqualTo(courseCode));
            Assert.That(result.TeacherId, Is.EqualTo(teacherId));
        }

        [Test]
        public void AssignTeacherForCourseOffering_WhenCourseDoesNotExists_ShouldThrowNoSuchCourseException()
        {
            //Arrange
            int teacherId = 2000;
            string courseCode = "CSE202";

            //Action
            var ex = Assert.ThrowsAsync<NoSuchCourseException>(() => courseOfferingService.AssignTeacherForCourseOffering(teacherId, courseCode));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such course found!"));
        }

        [Test]
        public void AssignTeacherForCourseOffering_WhenTeacherDoesNotExists_ShouldThrowNoSuchTeacherException()
        {
            //Arrange
            int teacherId = 1001;
            string courseCode = "CSE201";

            //Action
            var ex = Assert.ThrowsAsync<NoSuchTeacherException>(() => courseOfferingService.AssignTeacherForCourseOffering(teacherId, courseCode));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such teacher found!"));
        }

        [Test]
        public void AssignTeacherForCourseOffering_WhenCourseOfferingAlreadyExists_ShouldThrowCourseOfferingAlreadyExistsException()
        {
            //Arrange
            int teacherId = 2000;
            string courseCode = "CSE101";

            //Action
            var ex = Assert.ThrowsAsync<CourseOfferingAlreadyExistsException>(() => courseOfferingService.AssignTeacherForCourseOffering(teacherId, courseCode));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Course offering already exists!"));
        }

        [Test]
        public async Task GetAllCourseOfferings_WhenCourseOfferingsExists_ShouldReturnCourseOfferingDTOList()
        {
            //Action
            var result = await courseOfferingService.GetAllCourseOfferings();

            //Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(5));
        }

        [Test]
        public void GetAllCourseOfferings_WhenNoCourseOfferingsExists_ShouldThrowNoCourseOfferingException()
        {
            //Arrange
            context.CourseOfferings.RemoveRange(context.CourseOfferings);
            context.SaveChanges();

            //Action
            var ex = Assert.ThrowsAsync<NoCourseOfferingException>(() => courseOfferingService.GetAllCourseOfferings());

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No course offering found!"));
        }

        [Test]
        public async Task GetcourseOfferingByCourseCode_WhenCourseOfferingsExists_ShouldReturnCourseOfferingDTOList()
        {
            //Arrange
            string courseCode = "CSE101";


            //Action
            var result = await courseOfferingService.GetcourseOfferingByCourseCode(courseCode);

            //Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetcourseOfferingByCourseCode_WhenNoCourseOfferingsExists_ShouldThrowNoSuchCourseException()
        {
            //Arrange
            string courseCode = "CSE201";

            //Action
            var ex = Assert.ThrowsAsync<NoCourseOfferingException>(() => courseOfferingService.GetcourseOfferingByCourseCode(courseCode));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No course offering found!"));
        }

        [Test]
        public void GetcourseOfferingByCourseCode_WhenCourseDoesNotExists_ShouldThrowNoSuchCourseException()
        {
            //Arrange
            string courseCode = "CSE202";

            //Action
            var ex = Assert.ThrowsAsync<NoSuchCourseException>(() => courseOfferingService.GetcourseOfferingByCourseCode(courseCode));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such course found!"));
        }

        [Test]
        public async Task GetcourseOfferingByTeacherId_WhenCourseOfferingsExists_ShouldReturnCourseOfferingDTOList()
        {
            //Arrange
            int teacherId = 2000;

            //Action
            var result = await courseOfferingService.GetcourseOfferingByTeacherId(teacherId);

            //Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetcourseOfferingByTeacherId_WhenNoCourseOfferingsExists_ShouldThrowNoCourseOfferingException()
        {
            //Arrange
            context.CourseOfferings.RemoveRange(context.CourseOfferings);
            context.SaveChanges();

            int teacherId = 2000;

            //Action
            var ex = Assert.ThrowsAsync<NoCourseOfferingException>(() => courseOfferingService.GetcourseOfferingByTeacherId(teacherId));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No course offering found!"));
        }

        [Test]
        public void GetcourseOfferingByTeacherId_WhenTeacherDoesNotExists_ShouldThrowNoSuchTeacherException()
        {
            //Arrange
            int teacherId = 1001;

            //Action
            var ex = Assert.ThrowsAsync<NoSuchTeacherException>(() => courseOfferingService.GetcourseOfferingByTeacherId(teacherId));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such teacher found!"));
        }

        [Test]
        public async Task UnassignTeacherFromCourseOffering_WhenCourseOfferingExists()
        {
            //Arrange
            int teacherId = 2000;
            string courseCode = "CSE101";

            //Action
            var result = await courseOfferingService.UnassignTeacherFromCourseOffering(teacherId, courseCode);
            var isUpdated = context.CourseOfferings.Any(c => c.TeacherId == teacherId && c.CourseCode == courseCode);

            //Assert
            Assert.That(isUpdated, Is.EqualTo(false));
            Assert.NotNull(result);
        }

        [Test]
        public void UnassignTeacherFromCourseOffering_WhenCourseOfferingDoesNotExists_ShouldThrowNoSuchCourseOfferingException()
        {
            //Arrange
            int teacherId = 2000;
            string courseCode = "CSE201";

            //Action
            var ex = Assert.ThrowsAsync<NoSuchCourseOfferingException>(() => courseOfferingService.UnassignTeacherFromCourseOffering(teacherId, courseCode));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such course offering found."));
        }

        [Test]
        public void UnassignTeacherFromCourseOffering_WhenCourseDoesNotExists_ShouldThrowNoSuchCourseException()
        {
            //Arrange
            int teacherId = 2000;
            string courseCode = "CSE202";

            //Action
            var ex = Assert.ThrowsAsync<NoSuchCourseException>(() => courseOfferingService.UnassignTeacherFromCourseOffering(teacherId, courseCode));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such course found!"));
        }

        [Test]
        public void UnassignTeacherFromCourseOffering_WhenTeacherDoesNotExists_ShouldThrowNoSuchTeacherException()
        {
            //Arrange
            int teacherId = 1001;
            string courseCode = "CSE201";

            //Action
            var ex = Assert.ThrowsAsync<NoSuchTeacherException>(() => courseOfferingService.UnassignTeacherFromCourseOffering(teacherId, courseCode));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such teacher found!"));
        }

        [Test]
        public async Task UpdateTeacherForCourseOffering_WhenCourseOfferingExists_ShouldReturnCourseOfferingDTO()
        {
            //Arrange
            int teacherId = 2001;
            int couseOfferingId = 1;

            //Action
            var result = await courseOfferingService.UpdateTeacherForCourseOffering(teacherId, couseOfferingId);
            var isUpdated = await courseOfferingRepository.Get(couseOfferingId);

            //Assert
            Assert.NotNull(result);
            Assert.That(isUpdated.TeacherId, Is.EqualTo(teacherId));
        }

        [Test]
        public void UpdateTeacherForCourseOffering_WhenCourseOfferingDoesNotExists_ShouldThrowNoSuchCourseOfferingException()
        {
            //Arrange
            int teacherId = 2004;
            int couseOfferingId = 10;

            //Action
            var ex = Assert.ThrowsAsync<NoSuchCourseOfferingException>(() => courseOfferingService.UpdateTeacherForCourseOffering(teacherId, couseOfferingId));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such course offering found."));
        }

        [Test]
        public void UpdateTeacherForCourseOffering_WhenTeacherDoesNotExists_ShouldThrowNoSuchTeacherException()
        {
            //Arrange
            int teacherId = 1001;
            int couseOfferingId = 1;


            //Action
            var ex = Assert.ThrowsAsync<NoSuchTeacherException>(() => courseOfferingService.UpdateTeacherForCourseOffering(teacherId, couseOfferingId));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such teacher found!"));
        }
    }
}
