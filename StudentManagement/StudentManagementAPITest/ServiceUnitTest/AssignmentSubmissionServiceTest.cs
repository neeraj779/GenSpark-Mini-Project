using Microsoft.AspNetCore.Http;
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
    public class AssignmentSubmissionServiceTests
    {
        private StudentManagementContext context;
        private AssignmentSubmissionService assignmentSubmissionService;
        private IRepository<int, Submission> submissionRepository;
        private IRepository<int, Assignment> assignmentRepository;
        private IRepository<int, Student> studentRepository;
        private IRepository<int, Enrollment> enrollmentRepository;
        private IRepository<string, Course> courseRepository;
        private IUserRepository<int, User> userRepo;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            submissionRepository = new AssignmentSubmissionRepository(context);
            assignmentRepository = new AssignmentRepository(context);
            studentRepository = new StudentRepository(context);
            enrollmentRepository = new EnrollmentRepository(context);
            courseRepository = new CourseRepository(context);
            studentRepository = new StudentRepository(context);
            userRepo = new UserRepository(context);


            assignmentSubmissionService = new AssignmentSubmissionService(
                submissionRepository,
                assignmentRepository,
                studentRepository,
                enrollmentRepository,
                courseRepository
            );


            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new User { UserId = 1, Role = UserRole.Student };
            var student = new Student { UserId = 1, StudentId = 1 };
            var enrollment = new Enrollment { StudentId = 1, CourseCode = "CSE101" };
            var assignment = new Assignment { AssignmentId = 1, Title = "title", CourseCode = "CSE101" };
            context.Users.Add(user);
            context.Students.Add(student);
            context.Enrollments.Add(enrollment);
            context.Assignments.Add(assignment);
            context.SaveChanges();
        }


        [Test]
        public async Task SubmitAssignment_ValidSubmission_ReturnsAssignmentSubmissionReturnDTO()
        {
            // Arrange
            int userId = 1;
            var assignmentSubmission = new AssignmentSubmisssionDTO
            {
                AssignmentId = 1,
                File = new Mock<IFormFile>().Object
            };

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("assignment.pdf");
            fileMock.Setup(f => f.Length).Returns(1);
            assignmentSubmission.File = fileMock.Object;

            // Action
            var result = await assignmentSubmissionService.SubmitAssignment(userId, assignmentSubmission);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.AssignmentId, Is.EqualTo(assignmentSubmission.AssignmentId));
            Assert.That(result.FileName.Contains(".pdf"), Is.True);
        }

        [Test]
        public async Task SubmitAssignment_AlreadySubmitted_ThrowsDuplicateAssignmentSubmissionException()
        {
            // Arrange
            int userId = 1;
            var assignmentSubmission = new AssignmentSubmisssionDTO
            {
                AssignmentId = 1,
                File = new Mock<IFormFile>().Object
            };

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("assignment.pdf");
            fileMock.Setup(f => f.Length).Returns(1);
            assignmentSubmission.File = fileMock.Object;

            await assignmentSubmissionService.SubmitAssignment(userId, assignmentSubmission);

            // Action
            var ex = Assert.ThrowsAsync<DuplicateAssignmentSubmissionException>(() => assignmentSubmissionService.SubmitAssignment(userId, assignmentSubmission));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("You have already submitted this assignment."));
        }

        [Test]
        public void SubmitAssignment_NoSuchAssignment_ThrowsNoSuchAssignmentException()
        {
            // Arrange
            int userId = 1;
            var assignmentSubmission = new AssignmentSubmisssionDTO
            {
                AssignmentId = 9,
                File = new Mock<IFormFile>().Object
            };

            // Action
            var ex = Assert.ThrowsAsync<NoSuchAssignmentException>(() => assignmentSubmissionService.SubmitAssignment(userId, assignmentSubmission));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such assignment found!"));

        }

        [Test]
        public void SubmitAssignment_NoLinkedAccount_ThrowsNoLinkedAccountException()
        {
            // Arrange
            int userId = 2;
            var assignmentSubmission = new AssignmentSubmisssionDTO
            {
                AssignmentId = 1,
                File = new Mock<IFormFile>().Object
            };


            // Action
            var ex = Assert.ThrowsAsync<NoLinkedAccountException>(() => assignmentSubmissionService.SubmitAssignment(userId, assignmentSubmission));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Opps it seems you don't have a linked User Account. Please link your account first and try again."));
        }

        [Test]
        public void SubmitAssignment_NotEnrolledInCourse_ThrowsNotEnrolledInCourseException()
        {
            // Arrange
            int userId = 1;
            var assignmentSubmission = new AssignmentSubmisssionDTO
            {
                AssignmentId = 1,
                File = new Mock<IFormFile>().Object
            };

            context.Enrollments.RemoveRange(context.Enrollments);
            context.SaveChanges();

            // Action
            var ex = Assert.ThrowsAsync<NotEnrolledInCourseException>(() => assignmentSubmissionService.SubmitAssignment(userId, assignmentSubmission));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("You are not enrolled in this course."));
        }

        [Test]
        public void SubmitAssignment_InvalidFileExtension_ThrowsInvalidFileExtensionException()
        {
            // Arrange
            int userId = 1;
            var assignmentSubmission = new AssignmentSubmisssionDTO
            {
                AssignmentId = 1,
                File = new Mock<IFormFile>().Object
            };

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("assignment.txt");
            assignmentSubmission.File = fileMock.Object;

            // Action
            var ex = Assert.ThrowsAsync<InvalidFileExtensionException>(() => assignmentSubmissionService.SubmitAssignment(userId, assignmentSubmission));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Invalid file only .pdf files are allowed"));
        }

        [Test]
        public async Task GetAssignedAssignments_ValidStudent_ReturnsListOfAssignmentDTO()
        {
            // Arrange
            int userId = 1;

            // Action
            var result = await assignmentSubmissionService.GetAssignedAssignments(userId);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetAssignedAssignments_NoLinkedAccount_ThrowsNoLinkedAccountException()
        {
            // Arrange
            int userId = 2;

            // Action
            var ex = Assert.ThrowsAsync<NoLinkedAccountException>(() => assignmentSubmissionService.GetAssignedAssignments(userId));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Opps it seems you don't have a linked User Account. Please link your account first and try again."));
        }

        [Test]
        public void GetAssignedAssignments_NotEnrolledInCourse_ThrowsNotEnrolledInCourseException()
        {
            // Arrange
            int userId = 1;

            context.Enrollments.RemoveRange(context.Enrollments);
            context.SaveChanges();

            // Action
            var ex = Assert.ThrowsAsync<NotEnrolledInAnyCourseException>(() => assignmentSubmissionService.GetAssignedAssignments(userId));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("You are not enrolled in any course."));
        }

        [Test]
        public void GetAssignedAssignments_NoAssignmentFound_ThrowsNoAssignmentFoundException()
        {
            // Arrange
            int userId = 1;

            context.Assignments.RemoveRange(context.Assignments);
            context.SaveChanges();

            // Action
            var ex = Assert.ThrowsAsync<NoAssignmentFoundException>(() => assignmentSubmissionService.GetAssignedAssignments(userId));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No assignment found!"));
        }

        [Test]
        public async Task GetAssignedAssignmentsByCourse_ValidStudent_ReturnsListOfAssignmentDTO()
        {
            // Arrange
            int userId = 1;
            string courseCode = "CSE101";

            // Action
            var result = await assignmentSubmissionService.GetAssignedAssignmentsByCourse(userId, courseCode);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetAssignedAssignmentsByCourse_NoSuchCourse_ThrowsNoSuchCourseException()
        {
            // Arrange
            int userId = 1;
            string courseCode = "CSE204";

            // Action
            var ex = Assert.ThrowsAsync<NoSuchCourseException>(() => assignmentSubmissionService.GetAssignedAssignmentsByCourse(userId, courseCode));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such course found!"));
        }

        [Test]
        public void GetAssignedAssignmentsByCourse_NoLinkedAccount_ThrowsNoLinkedAccountException()
        {
            // Arrange
            int userId = 2;
            string courseCode = "CSE101";

            // Action
            var ex = Assert.ThrowsAsync<NoLinkedAccountException>(() => assignmentSubmissionService.GetAssignedAssignmentsByCourse(userId, courseCode));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Opps it seems you don't have a linked User Account. Please link your account first and try again."));
        }

        [Test]
        public void GetAssignedAssignmentsByCourse_NotEnrolledInCourse_ThrowsNotEnrolledInCourseException()
        {
            // Arrange
            int userId = 1;
            string courseCode = "CSE102";

            // Action
            var ex = Assert.ThrowsAsync<NotEnrolledInCourseException>(() => assignmentSubmissionService.GetAssignedAssignmentsByCourse(userId, courseCode));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("You are not enrolled in this course."));
        }

        [Test]
        public void GetAssignedAssignmentsByCourse_NoAssignmentFound_ThrowsNoAssignmentFoundException()
        {
            // Arrange
            context.Assignments.RemoveRange(context.Assignments);
            context.SaveChanges();

            int userId = 1;
            string courseCode = "CSE101";

            // Action
            var ex = Assert.ThrowsAsync<NoAssignmentFoundException>(() => assignmentSubmissionService.GetAssignedAssignmentsByCourse(userId, courseCode));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No assignment found!"));
        }

        [Test]
        public void GetAssignedAssignmentsByCourse_NoEnrollmentFound_ThrowsNotEnrolledInCourseException()
        {
            // Arrange
            int userId = 1;
            string courseCode = "CSE102";

            context.Enrollments.RemoveRange(context.Enrollments);
            context.SaveChanges();

            // Action
            var ex = Assert.ThrowsAsync<NotEnrolledInCourseException>(() => assignmentSubmissionService.GetAssignedAssignmentsByCourse(userId, courseCode));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("You are not enrolled in this course."));
        }

        [Test]
        public async Task GetAssignmentSubmissionStatus_ValidStudent_ReturnsAssignmentSubmissionReturnDTOAsync()
        {
            // Arrange
            int userId = 1;
            int assignmentId = 1;

            var assignmentSubmission = new AssignmentSubmisssionDTO
            {
                AssignmentId = 1,
                File = new Mock<IFormFile>().Object
            };

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("assignment.pdf");
            fileMock.Setup(f => f.Length).Returns(1);
            assignmentSubmission.File = fileMock.Object;

            await assignmentSubmissionService.SubmitAssignment(userId, assignmentSubmission);

            // Action
            var result = await assignmentSubmissionService.GetAssignmentSubmissionStatus(userId, assignmentId);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.AssignmentId, Is.EqualTo(assignmentId));
            Assert.That(result.FileName.Contains(".pdf"), Is.True);
        }

        [Test]
        public void GetAssignmentSubmissionStatus_NoSuchAssignmentSubmission_ThrowsNoSuchAssignmentSubmissionException()
        {
            // Arrange
            int userId = 1;
            int assignmentId = 2;

            // Action
            var ex = Assert.ThrowsAsync<NoSuchAssignmentSubmissionException>(() => assignmentSubmissionService.GetAssignmentSubmissionStatus(userId, assignmentId));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! Looks like the assignment submission you are looking for does not exist!"));
        }

        [Test]
        public void GetAssignmentSubmissionStatus_NoLinkedAccount_ThrowsNoLinkedAccountException()
        {
            // Arrange
            int userId = 2;
            int assignmentId = 1;

            // Action
            var ex = Assert.ThrowsAsync<NoLinkedAccountException>(() => assignmentSubmissionService.GetAssignmentSubmissionStatus(userId, assignmentId));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Opps it seems you don't have a linked User Account. Please link your account first and try again."));
        }


        [Test]
        public void GetSubmittedAssignment_validSubmission_ReturnsAssignmentSubmissionResultDTO()
        {
            // Arrange
            int assignmentId = 1;
            int studentId = 1;

            var assignmentSubmission = new AssignmentSubmisssionDTO
            {
                AssignmentId = 1,
                File = new Mock<IFormFile>().Object
            };

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("assignment.pdf");
            fileMock.Setup(f => f.Length).Returns(1);
            assignmentSubmission.File = fileMock.Object;

            var result = assignmentSubmissionService.SubmitAssignment(studentId, assignmentSubmission).Result;

            // Action
            var submissionResult = assignmentSubmissionService.GetSubmittedAssignment(assignmentId, studentId).Result;

            // Assert
            Assert.NotNull(submissionResult);
            Assert.That(submissionResult.FileName, Is.EqualTo(result.FileName));
        }

        [Test]
        public void GetSubmittedAssignment_NoSuchAssignment_ThrowsNoSuchAssignmentException()
        {
            // Arrange
            int assignmentId = 2;
            int studentId = 1;

            // Action
            var ex = Assert.ThrowsAsync<NoSuchAssignmentException>(() => assignmentSubmissionService.GetSubmittedAssignment(assignmentId, studentId));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("No such assignment found!"));
        }

        [Test]
        public void GetSubmittedAssignment_NoSuchStudent_ThrowsNoSuchStudentException()
        {
            // Arrange
            int assignmentId = 1;
            int studentId = 2;

            // Action
            var ex = Assert.ThrowsAsync<NoSuchStudentException>(() => assignmentSubmissionService.GetSubmittedAssignment(assignmentId, studentId));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such student found!"));
        }

        [Test]
        public void GetSubmittedAssignment_NoSuchAssignmentSubmission_ThrowsNoSuchAssignmentSubmissionException()
        {
            // Arrange
            int assignmentId = 1;
            int studentId = 1;

            // Action
            var ex = Assert.ThrowsAsync<NoSuchAssignmentSubmissionException>(() => assignmentSubmissionService.GetSubmittedAssignment(assignmentId, studentId));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! Looks like the assignment submission you are looking for does not exist!"));
        }
    }
}
