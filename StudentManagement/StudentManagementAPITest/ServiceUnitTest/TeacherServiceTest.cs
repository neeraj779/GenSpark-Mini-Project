using Microsoft.EntityFrameworkCore;
using Moq;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;
using StudentManagementAPI.Repositories;
using StudentManagementAPI.Services;

namespace StudentManagementAPITest.ServiceUnitTest
{
    public class TeacherServiceTest
    {
        StudentManagementContext context;
        IRepository<int, Teacher> teacherRepository;
        TeacherService teacherService;

        Mock<IRepository<int, Teacher>> _teacherRepositoryMock;
        TeacherService _teacherService;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            teacherRepository = new TeacherRepository(context);
            teacherService = new TeacherService(teacherRepository);

            _teacherRepositoryMock = new Mock<IRepository<int, Teacher>>();
            _teacherService = new TeacherService(_teacherRepositoryMock.Object);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public async Task TestRegisterTeacher()
        {
            //Arrange
            var newTeacher = new TeacherRegisterDTO
            {
                FullName = "Dr. Seema Reddy",
                Email = "seema.reddy@gmail.com",
                Gender = "Female",
                DateOfBirth = new DateTime(1983, 5, 20),
                Phone = "9876543216"
            };

            //Action
            var result = await teacherService.RegisterTeacher(newTeacher);

            //Assert
            Assert.That(result.FullName, Is.EqualTo(newTeacher.FullName));
        }
        
        [Test]
        public void RegisterTeacher_UnableToAdd_ThrowsUnableToAddException()
        {
            //Arrange
            var newTeacher = new TeacherRegisterDTO
            {
                FullName = "Dr. Seema Reddy",
                Email = "seema.reddy@gmail.com",
                Gender = "Female",
                DateOfBirth = new DateTime(1983, 5, 20),
                Phone = "9876543216"
            };

            //Action
            _teacherRepositoryMock.Setup(repo => repo.Add(It.IsAny<Teacher>())).ThrowsAsync(new UnableToAddException());

            // Assert
            var ex = Assert.ThrowsAsync<UnableToAddException>(async () => await _teacherService.RegisterTeacher(newTeacher));
            Assert.That(ex.Message, Is.EqualTo("Unable to Register Teacher. Please check the data and try again."));
        }

        [Test]
        public async Task TestUpdateTeacherEmail()
        {
            //Arrange
            var newTeacher = new UpdateEmailDTO
            {
                Id = 2000,
                Email = "unit@test.com"
            };

            //Action
            var result = await teacherService.UpdateTeacherEmail(newTeacher);

            var teacher = await teacherRepository.Get(newTeacher.Id);

            //Assert
            Assert.That(result.Email, Is.EqualTo(newTeacher.Email));
            Assert.That(teacher.Email, Is.EqualTo(newTeacher.Email));
        }


        [Test]
        public async Task TestUpdateTeacherEmailException()
        {
            //Arrange
            var newTeacher = new UpdateEmailDTO
            {
                Id = 203300,
                Email = "user@example.com"
            };

            //Action
            var ex = Assert.ThrowsAsync<NoSuchTeacherException>(() => teacherService.UpdateTeacherEmail(newTeacher));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such teacher found!"));
        }

        [Test]
        public async Task TestUpdateTeacherPhone()
        {
            //Arrange
            var newTeacher = new UpdatePhoneDTO
            {
                Id = 2000,
                Phone = "9876543210"
            };

            //Action
            var result = await teacherService.UpdateTeacherPhone(newTeacher);

            var teacher = await teacherRepository.Get(newTeacher.Id);

            //Assert
            Assert.That(result.Phone, Is.EqualTo(newTeacher.Phone));
            Assert.That(teacher.Phone, Is.EqualTo(newTeacher.Phone));
        }

        [Test]
        public async Task TestUpdateTeacherPhoneException()
        {
            //Arrange
            var newTeacher = new UpdatePhoneDTO
            {
                Id = 203300,
                Phone = "9876543210"
            };

            //Action
            var ex = Assert.ThrowsAsync<NoSuchTeacherException>(() => teacherService.UpdateTeacherPhone(newTeacher));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such teacher found!"));
        }

        [Test]
        public async Task TestGetAllTeachers()
        {
            //Arrange
            var teachers = await teacherService.GetTeachers();

            //Action
            var result = teachers.Count();

            //Assert
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public async Task TestGetAllTeachersException()
        {
            //Arrange
            context.Teachers.RemoveRange(context.Teachers);
            context.SaveChanges();

            //Action
            var ex = Assert.ThrowsAsync<NoTeacherFoundException>(() => teacherService.GetTeachers());

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No teacher found!"));
        }

        [Test]
        public async Task TestGetTeacherById()
        {
            //Arrange
            var teacher = await teacherService.GetTeacherById(2000);

            //Action
            var result = teacher.TeacherId;

            //Assert
            Assert.That(result, Is.EqualTo(2000));
        }

        [Test]
        public async Task TestGetTeacherByIdException()
        {
            //Action
            var ex = Assert.ThrowsAsync<NoSuchTeacherException>(() => teacherService.GetTeacherById(20000));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such teacher found!"));
        }

        [Test]
        public async Task TestDeleteTeacher()
        {
            //Arrange
            var teacher = await teacherService.DeleteTeacher(2000);
            var teachers = await teacherService.GetTeachers();

            //Action
            var result = teacher.TeacherId;

            //Assert
            Assert.That(result, Is.EqualTo(2000));
            Assert.That(teachers.Count(), Is.EqualTo(4));

        }

        [Test]
        public async Task TestDeleteTeacherFException()
        {
            // Arrange
            var teacher = await teacherService.DeleteTeacher(2000);

            // Action
            var ex = Assert.ThrowsAsync<NoSuchTeacherException>(() => teacherService.DeleteTeacher(2000));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such teacher found!"));
        }
    }
}
