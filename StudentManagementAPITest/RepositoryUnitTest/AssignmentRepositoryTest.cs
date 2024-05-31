using Microsoft.EntityFrameworkCore;
using Moq;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;

namespace StudentManagementAPITest.RepositoryUnitTest
{
    public class AssignmentRepositoryTest
    {
        StudentManagementContext context;
        private Mock<StudentManagementContext> mockContext;
        private AssignmentRepository mockAssignmentRepository;

        [SetUp]
        public async Task SetupAsync()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            mockContext = new Mock<StudentManagementContext>(optionsBuilder.Options);
            mockAssignmentRepository = new AssignmentRepository(mockContext.Object);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            IRepository<int, Assignment> assignmentRepository = new AssignmentRepository(context);
            Assignment assignment = new Assignment()
            {
                AssignmentId = 100,
                Title = "Test Assignment",
                CourseCode = "CSE101",
                DueDate = new DateTime(2024, 5, 6, 9, 0, 0),
            };

            await assignmentRepository.Add(assignment);
        }

        [Test]
        public async Task TestAddAssignment()
        {
            //Arrange
            IRepository<int, Assignment> assignmentRepository = new AssignmentRepository(context);
            Assignment assignment = new Assignment()
            {
                AssignmentId = 101,
                Title = "Add Test Assignment",
                CourseCode = "CSE102",
                DueDate = new DateTime(2024, 5, 6, 9, 0, 0),
            };

            //Action
            await assignmentRepository.Add(assignment);
            var assignmentResult = await assignmentRepository.Get(101);
            var allAssignments = await assignmentRepository.Get();

            //Assert
            Assert.That(assignmentResult.AssignmentId, Is.EqualTo(101));
            Assert.That(allAssignments.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Add_WhenDbUpdateExceptionThrown_ShouldThrowUnableToAddException()
        {
            // Arrange
            var assignment = new Assignment { AssignmentId = 1, Title = "Assignment 1", CourseCode = "CSE101" };
            mockContext.Setup(c => c.Add(assignment)).Verifiable();
            mockContext.Setup(c => c.SaveChangesAsync(default)).ThrowsAsync(new DbUpdateException());

            // Act and Assert
            var exception = Assert.ThrowsAsync<UnableToAddException>(async () => await mockAssignmentRepository.Add(assignment));
            Assert.AreEqual("Unable to add assignment. Please check the data and try again.", exception.Message);
        }

        [Test]
        public async Task TestGetAssignmentById()
        {
            //Arrange
            IRepository<int, Assignment> assignmentRepository = new AssignmentRepository(context);

            //Action
            var assignmentResult = await assignmentRepository.Get(100);

            //Assert
            Assert.That(assignmentResult.AssignmentId, Is.EqualTo(100));
        }

        [Test]
        public async Task TestGetAllAssignments()
        {
            //Arrange
            IRepository<int, Assignment> assignmentRepository = new AssignmentRepository(context);

            //Action
            var allAssignments = await assignmentRepository.Get();

            //Assert
            Assert.That(allAssignments.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task TestDeleteAssignment()
        {
            //Arrange
            IRepository<int, Assignment> assignmentRepository = new AssignmentRepository(context);

            //Action
            await assignmentRepository.Delete(100);
            var allAssignments = await assignmentRepository.Get();

            //Assert
            Assert.That(allAssignments.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestDeleteAssignment_InvalidAssignmentId()
        {
            //Arrange
            IRepository<int, Assignment> assignmentRepository = new AssignmentRepository(context);

            //Action and Assert
            Assert.ThrowsAsync<NoSuchAssignmentException>(() => assignmentRepository.Delete(101));
        }

        [Test]
        public async Task TestUpdateAssignment()
        {
            //Arrange
            IRepository<int, Assignment> assignmentRepository = new AssignmentRepository(context);

            //Action
            var assignment = await assignmentRepository.Get(100);
            assignment.Title = "Update Test Assignment";

            await assignmentRepository.Update(assignment);
            var assignmentResult = await assignmentRepository.Get(100);

            //Assert
            Assert.That(assignmentResult.Title, Is.EqualTo("Update Test Assignment"));
        }

        [Test]
        public void TestUpdateAssignment_InvalidAssignmentId()
        {
            //Arrange
            IRepository<int, Assignment> assignmentRepository = new AssignmentRepository(context);
            Assignment assignment = new Assignment()
            {
                AssignmentId = 101,
                Title = "Update Test Assignment",
                CourseCode = "CSE102",
                DueDate = new DateTime(2024, 5, 6, 9, 0, 0),
            };

            //Action and Assert
            Assert.ThrowsAsync<NoSuchAssignmentException>(() => assignmentRepository.Update(assignment));
        }
    }
}
