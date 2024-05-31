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
    public class StudentServiceTest
    {
        StudentManagementContext context;
        StudentService _studentService;
        IRepository<int, Student> studentRepository;
        private Mock<IRepository<int, Student>> _studentRepositoryMock;
        StudentService studentService;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            studentRepository = new StudentRepository(context);
            studentService = new StudentService(studentRepository);
            _studentRepositoryMock = new Mock<IRepository<int, Student>>();
            _studentService = new StudentService(_studentRepositoryMock.Object);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public async Task TestRegisterStudent()
        {
            //Arrange
            var newStudent = new StudentRegisterDTO
            {
                FullName = "John Doe",
                Email = "user@example.com",
                RollNo = "123456",
                Branch = "CSE",
                Degree = "B.Tech",
                Gender = "Male",
                DateOfBirth = new DateTime(1990, 5, 20),
                Phone = "9876543210",
                Status = StudentStatus.Graduated.ToString()
            };

            //Action
            var result = await studentService.RegisterStudent(newStudent);

            //Assert
            Assert.That(result.FullName, Is.EqualTo(newStudent.FullName));
        }

        [Test]
        public async Task TestRegisterStudentException()
        {
            //Arrange
            var newStudent = new StudentRegisterDTO
            {
                FullName = "John Doe",
                Email = "user@example.com",
                RollNo = "123456",
                Branch = "CSE",
                Degree = "B.Tech",
                Gender = "Male",
                DateOfBirth = new DateTime(1990, 5, 20),
                Phone = "9876543210",
                Status = "wrong status"
            };

            //Action
            var exception = Assert.ThrowsAsync<InvalidStudentStatusException>(() => studentService.RegisterStudent(newStudent));

            //Assert
            Assert.That(exception.Message, Is.EqualTo("Invalid Student Status, Please provide a valid status. valid status are Undergraduate, Postgraduate, Alumni, Graduated, DroppedOut, Expelled, Suspended and Transferred"));
        }

        [Test]
        public void RegisterStudent_UnableToAdd_ThrowsUnableToAddException()
        {
            // Arrange
            var student = new StudentRegisterDTO
            {
                FullName = "Jack Doe",
                RollNo = "12347",
                Gender = "Male",
                Department = "Commerce",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Phone = "1122334455",
                Email = "jack.doe@example.com",
                Status = "Graduated"
            };

            _studentRepositoryMock.Setup(repo => repo.Add(It.IsAny<Student>())).ThrowsAsync(new UnableToAddException());

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnableToAddException>(async () => await _studentService.RegisterStudent(student));
            Assert.That(ex.Message, Is.EqualTo("Unable to Register Student. Please check the data and try again."));
        }

        [Test]
        public async Task TestUpdateStudentEmail()
        {
            //Arrange
            var newStudent = new UpdateEmailDTO
            {
                Id = 4000,
                Email = "test@example.com"
            };

            //Action
            var result = await studentService.UpdateStudentEmail(newStudent);
            var student = await studentRepository.Get(newStudent.Id);

            //Assert
            Assert.That(result.Email, Is.EqualTo(newStudent.Email));
            Assert.That(student.Email, Is.EqualTo(newStudent.Email));
        }

        [Test]
        public async Task TestUpdateStudentEmailException()
        {
            //Arrange
            var newStudent = new UpdateEmailDTO
            {
                Id = 5000,
                Email = "user@example.com"
            };

            //Action
            var exception = Assert.ThrowsAsync<NoSuchStudentException>(() => studentService.UpdateStudentEmail(newStudent));

            //Assert
            Assert.That(exception.Message, Is.EqualTo("Uh oh! No such student found!"));
        }

        [Test]
        public async Task TestUpdateStudentPhone()
        {
            //Arrange
            var newStudent = new UpdatePhoneDTO
            {
                Id = 4000,
                Phone = "9876543210"
            };

            //Action
            var result = await studentService.UpdateStudentPhone(newStudent);
            var student = await studentRepository.Get(newStudent.Id);

            //Assert
            Assert.That(result.Phone, Is.EqualTo(newStudent.Phone));
            Assert.That(student.Phone, Is.EqualTo(newStudent.Phone));
        }

        [Test]
        public async Task TestUpdateStudentPhoneException()
        {
            //Arrange
            var newStudent = new UpdatePhoneDTO
            {
                Id = 5000,
                Phone = "9876543210"
            };

            //Action
            var exception = Assert.ThrowsAsync<NoSuchStudentException>(() => studentService.UpdateStudentPhone(newStudent));

            //Assert
            Assert.That(exception.Message, Is.EqualTo("Uh oh! No such student found!"));
        }

        [Test]
        public async Task TestUpdateStudentStatus()
        {
            //Arrange
            int Id = 4000;
            string status = StudentStatus.Alumni.ToString();

            //Action
            var result = await studentService.UpdateStudentStatus(Id, status);
            var student = await studentRepository.Get(Id);

            //Assert
            Assert.That(result.Status, Is.EqualTo(status));
            Assert.That(student.Status, Is.EqualTo(Enum.Parse<StudentStatus>(status)));
        }

        [Test]
        public async Task TestUpdateStudentStatusException()
        {
            //Arrange
            int Id = 4000;
            string status = StudentStatus.Alumni.ToString();

            int wrongId = 5000;
            string wrongStatus = "InvalidStatus";

            //Action
            var exception = Assert.ThrowsAsync<NoSuchStudentException>(() => studentService.UpdateStudentStatus(wrongId, status));
            var exception2 = Assert.ThrowsAsync<InvalidStudentStatusException>(() => studentService.UpdateStudentStatus(Id, wrongStatus));

            //Assert
            Assert.That(exception.Message, Is.EqualTo("Uh oh! No such student found!"));
            Assert.That(exception2.Message, Is.EqualTo("Invalid Student Status, Please provide a valid status. valid status are Undergraduate, Postgraduate, Alumni, Graduated, DroppedOut, Expelled, Suspended and Transferred"));
        }

        [Test]
        public async Task TestGetAllStudents()
        {
            //Arrange
            var students = await studentService.GetStudents();

            //Action
            var result = students.Count();

            //Assert
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public async Task TestGetAllStudentsException()
        {
            //Arrange
            context.Students.RemoveRange(context.Students);
            context.SaveChanges();

            //Action
            var exception = Assert.ThrowsAsync<NoStudentFoundException>(() => studentService.GetStudents());

            //Assert
            Assert.That(exception.Message, Is.EqualTo("Uh oh! No student found!"));
        }

        [Test]
        public async Task TestGetStudentById()
        {
            //Arrange
            var studentId = 4000;

            //Action
            var student = await studentService.GetStudentById(studentId);

            //Assert
            Assert.That(student.StudentId, Is.EqualTo(studentId));
        }

        [Test]
        public async Task TestGetStudentByIdException()
        {
            //Arrange
            var studentId = 5000;

            //Action
            var exception = Assert.ThrowsAsync<NoSuchStudentException>(() => studentService.GetStudentById(studentId));

            //Assert
            Assert.That(exception.Message, Is.EqualTo("Uh oh! No such student found!"));
        }

        [Test]
        public async Task TestDeleteStudent()
        {
            //Arrange
            var studentId = 4000;

            //Action
            var student = await studentService.DeleteStudent(studentId);

            //Assert
            Assert.That(student.StudentId, Is.EqualTo(studentId));
        }

        [Test]
        public async Task TestDeleteStudentException()
        {
            //Arrange
            var studentId = 5000;

            //Action
            var exception = Assert.ThrowsAsync<NoSuchStudentException>(() => studentService.DeleteStudent(studentId));

            //Assert
            Assert.That(exception.Message, Is.EqualTo("Uh oh! No such student found!"));
        }
    }
}
