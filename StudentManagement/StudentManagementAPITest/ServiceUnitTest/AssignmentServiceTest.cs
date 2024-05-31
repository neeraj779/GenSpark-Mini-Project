using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;
using StudentManagementAPI.Repositories;
using StudentManagementAPI.Services;

namespace StudentManagementAPITest.ServiceUnitTest
{
    internal class AssignmentServiceTest
    {
        private StudentManagementContext context;
        private IRepository<int, Assignment> assignmentRepository;
        private IRepository<string, Course> courseRepository;
        private AssignmentService assignmentService;


        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            assignmentRepository = new AssignmentRepository(context);
            courseRepository = new CourseRepository(context);

            assignmentService = new AssignmentService(assignmentRepository, courseRepository);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public async Task CreateAssignment_ValidAssignment_CreatesAssignment()
        {
            // Arrange
            var assignment = new CreateAssignmentDTO
            {
                Title = "Assignment 1",
                AssignmentDueDate = DateTime.Now,
                CourseCode = "CSE201"
            };

            // Act
            var result = await assignmentService.CreateAssignment(assignment);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Title, Is.EqualTo(assignment.Title));
            Assert.That(result.AssignmentDueDate, Is.EqualTo(assignment.AssignmentDueDate));
            Assert.That(result.CourseCode, Is.EqualTo(assignment.CourseCode));
        }

        [Test]
        public void CreateAssignment_CourseDoesNotExist_ThrowsNoSuchCourseException()
        {
            // Arrange
            var assignment = new CreateAssignmentDTO
            {
                Title = "Assignment 1",
                AssignmentDueDate = DateTime.Now,
                CourseCode = "CS101"
            };

            // Action
            var ex = Assert.ThrowsAsync<NoSuchCourseException>(() => assignmentService.CreateAssignment(assignment));
            Assert.That(ex.Message, Is.EqualTo("No such course found!"));
        }

        [Test]
        public async Task CreateAssignment_AssignmentAlreadyExists_ThrowsAssignmentAlreadyExistsExceptionAsync()
        {
            // Arrange
            var assignment = new CreateAssignmentDTO
            {
                Title = "Assignment 1",
                AssignmentDueDate = DateTime.Now,
                CourseCode = "CSE201"
            };

            // Action
            await assignmentService.CreateAssignment(assignment);
            var ex = Assert.ThrowsAsync<AssignmentAlreadyExistsException>(() => assignmentService.CreateAssignment(assignment));
            Assert.That(ex.Message, Is.EqualTo("Uh oh! Assignment already exists!"));
        }

        [Test]
        public async Task DeleteAssignment_ValidAssignment_DeletesAssignment()
        {
            // Arrange
            var assignment = new CreateAssignmentDTO
            {
                Title = "Assignment 1",
                AssignmentDueDate = DateTime.Now,
                CourseCode = "CSE201"
            };

            var createdAssignment = await assignmentService.CreateAssignment(assignment);

            // Act
            var result = await assignmentService.DeleteAssignment(createdAssignment.AssignmentId);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Title, Is.EqualTo(assignment.Title));
        }

        [Test]
        public void DeleteAssignment_AssignmentDoesNotExist_ThrowsNoSuchAssignmentException()
        {
            // Arrange
            int assignmentId = 9999;

            // Action
            var ex = Assert.ThrowsAsync<NoSuchAssignmentException>(() => assignmentService.DeleteAssignment(assignmentId));
            Assert.That(ex.Message, Is.EqualTo("No such assignment found!"));
        }

        [Test]
        public async Task GetAssignmentById_ValidAssignmentId_ReturnsAssignment()
        {
            // Arrange
            var assignment = new CreateAssignmentDTO
            {
                Title = "Assignment 1",
                AssignmentDueDate = DateTime.Now,
                CourseCode = "CSE201"
            };

            var createdAssignment = await assignmentService.CreateAssignment(assignment);

            // Act
            var result = await assignmentService.GetAssignmentById(createdAssignment.AssignmentId);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Title, Is.EqualTo(assignment.Title));
        }

        [Test]
        public void GetAssignmentById_AssignmentDoesNotExist_ThrowsNoSuchAssignmentException()
        {
            // Arrange
            int assignmentId = 9999;

            // Action
            var ex = Assert.ThrowsAsync<NoSuchAssignmentException>(() => assignmentService.GetAssignmentById(assignmentId));
            Assert.That(ex.Message, Is.EqualTo("No such assignment found!"));
        }

        [Test]
        public async Task GetAssignments_ValidAssignments_ReturnsAssignments()
        {
            // Arrange
            var assignment1 = new CreateAssignmentDTO
            {
                Title = "Assignment 1",
                AssignmentDueDate = DateTime.Now,
                CourseCode = "CSE201"
            };

            var assignment2 = new CreateAssignmentDTO
            {
                Title = "Assignment 2",
                AssignmentDueDate = DateTime.Now,
                CourseCode = "CSE201"
            };

            var createdAssignment1 = await assignmentService.CreateAssignment(assignment1);
            var createdAssignment2 = await assignmentService.CreateAssignment(assignment2);

            // Act
            var result = await assignmentService.GetAssignments();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetAssignments_NoAssignments_ThrowsNoAssignmentFoundException()
        {
            // Arrange
            context.Assignments.RemoveRange(context.Assignments);
            context.SaveChanges();

            // Action
            var ex = Assert.ThrowsAsync<NoAssignmentFoundException>(() => assignmentService.GetAssignments());
            Assert.That(ex.Message, Is.EqualTo("No assignment found!"));
        }

        [Test]
        public async Task UpdateAssignmentDueDate_ValidAssignment_ReturnsUpdatedAssignment()
        {
            // Arrange
            var assignment = new CreateAssignmentDTO
            {
                Title = "Assignment 1",
                AssignmentDueDate = DateTime.Now,
                CourseCode = "CSE201"
            };

            var createdAssignment = await assignmentService.CreateAssignment(assignment);

            var assignmentUpdate = new AssignmentUpdateDTO
            {
                AssignmentId = createdAssignment.AssignmentId,
                AssignmentDueDate = DateTime.Now.AddDays(1)
            };

            // Action
            var result = await assignmentService.UpdateAssignmentDueDate(assignmentUpdate);
            var updatedAssignment = await assignmentService.GetAssignmentById(createdAssignment.AssignmentId);

            // Assert
            Assert.NotNull(result);
            Assert.That(updatedAssignment.AssignmentDueDate, Is.EqualTo(result.AssignmentDueDate));
        }

        [Test]
        public void UpdateAssignmentDueDate_AssignmentDoesNotExist_ThrowsNoSuchAssignmentException()
        {
            // Arrange
            var assignmentUpdate = new AssignmentUpdateDTO
            {
                AssignmentId = 9999,
                AssignmentDueDate = DateTime.Now.AddDays(1)
            };

            // Action
            var ex = Assert.ThrowsAsync<NoSuchAssignmentException>(() => assignmentService.UpdateAssignmentDueDate(assignmentUpdate));
            Assert.That(ex.Message, Is.EqualTo("No such assignment found!"));
        }
    }
}
