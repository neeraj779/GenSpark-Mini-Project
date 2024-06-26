﻿using Microsoft.EntityFrameworkCore;
using Moq;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;

namespace StudentManagementAPITest.RepositoryUnitTest
{
    public class StudentReposiotryTest
    {
        StudentManagementContext context;
        private Mock<StudentManagementContext> mockContext;
        private StudentRepository mockStudentRepository;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            mockContext = new Mock<StudentManagementContext>(optionsBuilder.Options);
            mockStudentRepository = new StudentRepository(mockContext.Object);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public async Task TestAddStudent()
        {
            //Arrange
            IRepository<int, Student> studentRepository = new StudentRepository(context);
            Student student = new Student()
            {
                StudentId = 1,
                FullName = "Mr. Raj Patel",
                RollNo = "CSE2020002",
                Department = "Computer Science",
                Email = "raj.patel@gmail.com",
                Gender = "Male",
                Phone = "9374729562",
                Status = StudentStatus.Undergraduate,
                DateOfBirth = new DateTime(1998, 2, 15),
                User = null,
                Enrollments = null,
                ClassAttendances = null,
                Submissions = null
            };

            //Action
            await studentRepository.Add(student);
            var studentResult = await studentRepository.Get(1);

            //Assert
            Assert.That(studentResult.StudentId, Is.EqualTo(1));
        }

        [Test]
        public void Add_WhenDbUpdateExceptionThrown_ShouldThrowUnableToAddException()
        {
            //Arrange
            IRepository<int, Student> studentRepository = new StudentRepository(context);
            Student student = new Student()
            {
                FullName = "Mr. Raj Patel",
                RollNo = "CSE2020002",
                Department = "Computer Science",
            };

            //Action
            mockContext.Setup(c => c.Add(student)).Verifiable();
            mockContext.Setup(c => c.SaveChangesAsync(default)).ThrowsAsync(new DbUpdateException());

            //Assert
            var exception = Assert.ThrowsAsync<UnableToAddException>(async () => await mockStudentRepository.Add(student));
            Assert.That(exception.Message, Is.EqualTo("Unable to add student. Please check the data and try again."));
        }

        [Test]
        public async Task TestGetStudentById()
        {
            //Arrange
            IRepository<int, Student> studentRepository = new StudentRepository(context);

            //Action
            var studentResult = await studentRepository.Get(4000);

            //Assert
            Assert.That(studentResult.StudentId, Is.EqualTo(4000));
        }

        [Test]
        public async Task TestGetAllStudents()
        {
            //Arrange
            IRepository<int, Student> studentRepository = new StudentRepository(context);

            //Action
            var studentResult = await studentRepository.Get();

            //Assert
            Assert.That(studentResult.Count(), Is.EqualTo(5));
        }

        [Test]
        public async Task TestUpdateStudent()
        {
            //Arrange
            IRepository<int, Student> studentRepository = new StudentRepository(context);

            //Action
            var studentResult = await studentRepository.Get(4000);
            studentResult.FullName = "Updated Student";

            await studentRepository.Update(studentResult);

            var student = await studentRepository.Get(4000);

            //Assert
            Assert.That(student.FullName, Is.EqualTo("Updated Student"));
        }

        [Test]
        public void TestUpdateStudentNotFound()
        {
            //Arrange
            IRepository<int, Student> studentRepository = new StudentRepository(context);
            Student student = new Student()
            {
                StudentId = 1,
                FullName = "Mr. Raj Patel",
                RollNo = "CSE2020002",
                Department = "Computer Science",
                Email = "raj.patel@gmail.com",
                Gender = "Male",
                Phone = "9374729562",
                Status = StudentStatus.Undergraduate,
                DateOfBirth = new DateTime(1998, 2, 15)
            };

            //Action
            var ex = Assert.ThrowsAsync<NoSuchStudentException>(() => studentRepository.Update(student));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such student found!"));
        }

        [Test]
        public void TestDeleteStudentNotFound()
        {
            //Arrange
            IRepository<int, Student> studentRepository = new StudentRepository(context);

            //Action
            var ex = Assert.ThrowsAsync<NoSuchStudentException>(() => studentRepository.Delete(1000));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such student found!"));
        }

        [Test]
        public async Task TestDeleteStudent()
        {
            //Arrange
            IRepository<int, Student> studentRepository = new StudentRepository(context);

            //Action
            await studentRepository.Delete(4000);

            var student = await studentRepository.Get(4000);

            //Assert
            Assert.That(student, Is.Null);
        }
    }
}
