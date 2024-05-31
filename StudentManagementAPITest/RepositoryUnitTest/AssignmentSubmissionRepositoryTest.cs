using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;

namespace StudentManagementAPITest.RepositoryUnitTest
{
    public class AssignmentSubmissionRepositoryTest
    {
        StudentManagementContext context;
        [SetUp]
        public async Task SetupAsync()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();


            IRepository<int, Submission> assignmentSubmissionRepository = new AssignmentSubmissionRepository(context);

            Submission assignmentSubmission = new Submission()
            {
                AssignmentId = 1,
                StudentId = 4000,
                FileName = "Assignment1.pdf",
                SubmissionDate = new DateTime(2024, 5, 6, 9, 0, 0),
            };

            await assignmentSubmissionRepository.Add(assignmentSubmission);
        }

        [Test]
        public async Task TestAddAssignmentSubmission()
        {
            //Arrange
            IRepository<int, Submission> assignmentSubmissionRepository = new AssignmentSubmissionRepository(context);

            Submission assignmentSubmission = new Submission()
            {
                AssignmentId = 2,
                StudentId = 4001,
                FileName = "Assignment1.pdf",
                SubmissionDate = new DateTime(2024, 5, 6, 9, 0, 0),
            };


            //Action
            await assignmentSubmissionRepository.Add(assignmentSubmission);
            var assignmentSubmissionResult = await assignmentSubmissionRepository.Get(2);

            //Assert
            Assert.That(assignmentSubmissionResult.SubmissionId, Is.EqualTo(2));
        }

        [Test]
        public async Task TestGetAssignmentSubmissionById()
        {
            //Arrange
            IRepository<int, Submission> assignmentSubmissionRepository = new AssignmentSubmissionRepository(context);

            //Action
            var assignmentSubmissionResult = await assignmentSubmissionRepository.Get(1);

            //Assert
            Assert.That(assignmentSubmissionResult.SubmissionId, Is.EqualTo(1));
        }

        [Test]
        public async Task TestGetAllAssignmentSubmissions()
        {
            //Arrange
            IRepository<int, Submission> assignmentSubmissionRepository = new AssignmentSubmissionRepository(context);

            //Action
            var assignmentSubmissionResult = await assignmentSubmissionRepository.Get();

            //Assert
            Assert.That(assignmentSubmissionResult.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task TestDeleteAssignmentSubmission()
        {
            //Arrange
            IRepository<int, Submission> assignmentSubmissionRepository = new AssignmentSubmissionRepository(context);

            //Action
            var result = await assignmentSubmissionRepository.Delete(1);
            var resultAll = await assignmentSubmissionRepository.Get();

            //Assert
            Assert.That(resultAll.Count, Is.EqualTo(0));
            Assert.That(result.SubmissionId, Is.EqualTo(1));
        }

        [Test]
        public async Task TestUpdateAssignmentSubmission()
        {
            //Arrange
            IRepository<int, Submission> assignmentSubmissionRepository = new AssignmentSubmissionRepository(context);

            //Action
            var assignmentSubmissionResult = await assignmentSubmissionRepository.Get(1);
            assignmentSubmissionResult.FileName = "Updated Assignment";

            await assignmentSubmissionRepository.Update(assignmentSubmissionResult);

            var assignmentSubmission = await assignmentSubmissionRepository.Get(1);

            //Assert
            Assert.That(assignmentSubmission.FileName, Is.EqualTo("Updated Assignment"));
        }

    }
}
