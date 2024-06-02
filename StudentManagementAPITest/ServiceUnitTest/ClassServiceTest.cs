using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;
using StudentManagementAPI.Repositories;
using StudentManagementAPI.Services;

namespace StudentManagementAPITest.ServiceUnitTest
{
    internal class ClassServiceTest
    {
        StudentManagementContext context;
        IRepository<int, Class> classRepository;
        IRepository<int, Teacher> teacherRepository;
        IRepository<string, Course> courseRepository;
        IRepository<int, CourseOffering> courseOfferingRepository;
        ClassService classService;



        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            classRepository = new ClassRepository(context);
            teacherRepository = new TeacherRepository(context);
            courseRepository = new CourseRepository(context);
            courseOfferingRepository = new CourseOfferingRepository(context);

            classService = new ClassService(classRepository, teacherRepository, courseRepository, courseOfferingRepository);


            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public async Task AddClassTest()
        {
            //Arrange
            var classdto = new ClassRegisterDTO
            {
                CourseOfferingId = 1,
                ClassDateAndTime = DateTime.Now
            };

            // Act
            var result = await classService.AddClass(classdto);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.ClassId, Is.EqualTo(11));
            Assert.That(result.CourseOfferingId, Is.EqualTo(1));
            Assert.That(result.ClassDateAndTime, Is.EqualTo(classdto.ClassDateAndTime));
        }

        [Test]
        public async Task AddClassTest_ClassAlreadyExistsException()
        {
            //Arrange
            var classdto = new ClassRegisterDTO
            {
                CourseOfferingId = 1,
                ClassDateAndTime = DateTime.Now
            };

            // Act
            await classService.AddClass(classdto);
            var ex = Assert.ThrowsAsync<ClassAlreadyExistsException>(() => classService.AddClass(classdto));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Class already exists!"));
        }

        [Test]
        public void AddClassTest_NoSuchCourseOfferingException()
        {
            //Arrange
            var classdto = new ClassRegisterDTO
            {
                CourseOfferingId = 15,
                ClassDateAndTime = DateTime.Now
            };

            // Action
            var ex = Assert.ThrowsAsync<NoSuchCourseOfferingException>(() => classService.AddClass(classdto));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No such course offering found."));
        }

        [Test]
        public async Task DeleteClassTest()
        {
            //Arrange
            int classId = 1;

            // Action
            var deletedClass = await classService.DeleteClass(classId);
            var totalClasses = await classRepository.Get();

            // Assert
            Assert.NotNull(deletedClass);
            Assert.That(deletedClass.ClassId, Is.EqualTo(1));
            Assert.That(deletedClass.CourseOfferingId, Is.EqualTo(1));
            Assert.That(totalClasses.Count(), Is.EqualTo(9));
        }

        [Test]
        public void DeleteClassTest_NoSuchClassException()
        {
            //Arrange
            int classId = 150;

            // Action
            var ex = Assert.ThrowsAsync<NoSuchClassException>(() => classService.DeleteClass(classId));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Class with given Id does not exist."));
        }

        [Test]
        public async Task GetClassTest()
        {
            //Arrange
            int classId = 1;

            // Action
            var classObj = await classService.GetClass(classId);

            // Assert
            Assert.NotNull(classObj);
            Assert.That(classObj.ClassId, Is.EqualTo(1));
            Assert.That(classObj.CourseOfferingId, Is.EqualTo(1));
        }

        [Test]
        public void GetClassTest_NoSuchClassException()
        {
            //Arrange
            int classId = 150;

            // Action
            var ex = Assert.ThrowsAsync<NoSuchClassException>(() => classService.GetClass(classId));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Class with given Id does not exist."));

        }

        [Test]
        public async Task GetClassesTest()
        {
            // Action
            var classes = await classService.GetClasses();

            // Assert
            Assert.NotNull(classes);
            Assert.That(classes.Count(), Is.EqualTo(10));
        }

        [Test]
        public void GetClassesTest_NoClassFoundException()
        {
            //Arrange
            context.Classes.RemoveRange(context.Classes);
            context.SaveChanges();

            // Action
            var ex = Assert.ThrowsAsync<NoClassFoundException>(() => classService.GetClasses());

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No class found!"));
        }

        [Test]
        public async Task UpdateClassTimeTest()
        {
            //Arrange
            var updateclassdto = new UpdateClassDTO
            {
                ClassId = 1,
                ClassDateAndTime = DateTime.Now
            };

            // Action
            var updatedClass = await classService.UpdateClassTime(updateclassdto);
            var isUpdated = await classRepository.Get(1);

            // Assert
            Assert.NotNull(updatedClass);
            Assert.That(updatedClass.ClassId, Is.EqualTo(1));
            Assert.That(updatedClass.CourseOfferingId, Is.EqualTo(1));
            Assert.That(updatedClass.ClassDateAndTime, Is.EqualTo(updateclassdto.ClassDateAndTime));
            Assert.IsTrue(isUpdated.ClassDateAndTime == updateclassdto.ClassDateAndTime);
        }

        [Test]
        public void UpdateClassTimeTest_NoSuchClassException()
        {
            //Arrange
            var updateclassdto = new UpdateClassDTO
            {
                ClassId = 150,
                ClassDateAndTime = DateTime.Now
            };

            // Action
            var ex = Assert.ThrowsAsync<NoSuchClassException>(() => classService.UpdateClassTime(updateclassdto));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Class with given Id does not exist."));
        }
    }
}
