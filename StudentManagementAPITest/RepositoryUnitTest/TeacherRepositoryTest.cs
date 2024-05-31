using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;

namespace StudentManagementAPITest.RepositoryUnitTest
{
    public class TeacherRepositoryTest
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
        public async Task TestAddTeacher()
        {
            //Arrange
            IRepository<int, Teacher> teacherRepository = new TeacherRepository(context);
            Teacher teacher = new Teacher()
            {
                TeacherId = 1371731,
                FullName = "Test Teacher",
                Email = "example@test.com",
                Gender = "Male",
                Phone = "1234567890",
            };

            //Action
            await teacherRepository.Add(teacher);
            var teacherResult = await teacherRepository.Get(1371731);

            //Assert
            Assert.That(teacherResult.TeacherId, Is.EqualTo(1371731));
        }


        [Test]
        public async Task TestGetTeacherById()
        {
            //Arrange
            IRepository<int, Teacher> teacherRepository = new TeacherRepository(context);
            //Action
            var teacherResult = await teacherRepository.Get(2000);

            //Assert
            Assert.That(teacherResult.TeacherId, Is.EqualTo(2000));
        }

        [Test]
        public async Task TestGetTeacherByIdNotFound()
        {
            //Arrange
            IRepository<int, Teacher> teacherRepository = new TeacherRepository(context);
            //Action
            var teacherResult = await teacherRepository.Get(1000);

            //Assert
            Assert.That(teacherResult, Is.Null);
        }

        [Test]
        public async Task TestGetAllTeachers()
        {
            //Arrange
            IRepository<int, Teacher> teacherRepository = new TeacherRepository(context);

            //Action
            var teacherResult = await teacherRepository.Get();

            //Assert
            Assert.That(teacherResult.Count, Is.EqualTo(5));
        }

        [Test]
        public async Task TestUpdateTeacher()
        {
            //Arrange
            IRepository<int, Teacher> teacherRepository = new TeacherRepository(context);

            //Action
            var teacher = await teacherRepository.Get(2000);
            teacher.FullName = "Updated Teacher";

            await teacherRepository.Update(teacher);
            var teacherResult = await teacherRepository.Get(2000);

            //Assert
            Assert.That(teacherResult.FullName, Is.EqualTo("Updated Teacher"));
        }

        [Test]
        public void TestUpdateTeacherNotFound()
        {
            //Arrange
            IRepository<int, Teacher> teacherRepository = new TeacherRepository(context);
            Teacher teacher = new Teacher()
            {
                TeacherId = 1371731,
                FullName = "Test Teacher",
                Email = "example@test.com",
                Gender = "Male",
                Phone = "1234567890",
            };

            //Action
            var ex = Assert.ThrowsAsync<NoSuchTeacherException>(() => teacherRepository.Update(teacher));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such teacher found!"));
        }

        [Test]
        public async Task TestDeleteTeacher()
        {
            //Arrange
            IRepository<int, Teacher> teacherRepository = new TeacherRepository(context);

            //Action
            await teacherRepository.Delete(2000);
            var teacherResult = await teacherRepository.Get(2000);

            //Assert
            Assert.That(teacherResult, Is.Null);
        }

        [Test]
        public void TestDeleteTeacherNotFound()
        {
            //Arrange
            IRepository<int, Teacher> teacherRepository = new TeacherRepository(context);

            //Action
            var ex = Assert.ThrowsAsync<NoSuchTeacherException>(() => teacherRepository.Delete(1000));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such teacher found!"));
        }
    }
}