using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;
using StudentManagementAPI.Exceptions;
using System.Security.Cryptography;
using System.Text;

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
            Teacher teacher = new Teacher()
            {
                TeacherId = 1,
                FullName = "Test Teacher",
                Email = "example@test.com",
                Gender = "Male",
                Phone = "1234567890",
            };

            //Action
            await teacherRepository.Add(teacher);
            var teacherResult = await teacherRepository.Get(1);

            //Assert
            Assert.That(teacherResult.TeacherId, Is.EqualTo(1));
        }
    }
}