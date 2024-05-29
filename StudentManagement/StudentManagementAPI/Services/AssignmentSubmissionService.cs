using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;
using System.Security.Claims;

namespace StudentManagementAPI.Services
{
    public class AssignmentSubmissionService : IAssignmentSubmissionService
    {
        public readonly IRepository<int, Submission> _sumissionRepository;
        public readonly IRepository<int, Assignment> _assignmentRepository;
        public readonly IRepository<int, Student> _studentRepository;
        public readonly IRepository<int, Enrollment> _enrollmentRepository;
        public readonly IRepository<string, Course> _courseRepository;


        public AssignmentSubmissionService(
            IRepository<int, Submission> sumissionRepository,
            IRepository<int, Assignment> assignmentRepository,
            IRepository<int, Student> studentRepository,
            IRepository<int, Enrollment> enrollmentRepository,
            IRepository<string, Course> courseRepository
            )
        {
            _sumissionRepository = sumissionRepository;
            _assignmentRepository = assignmentRepository;
            _studentRepository = studentRepository;
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
        }

        public async Task<AssignmentSubmisssionReturnDTO> SubmitAssignment(int userId, AssignmentSubmisssionDTO assignmentSubmission)
        {
            var isAssignmentExists = await _assignmentRepository.Get(assignmentSubmission.AssignmentId);
            if (isAssignmentExists == null)
                throw new NoSuchAssignmentException();

            var studentDb = await _studentRepository.Get();
            var student = studentDb.FirstOrDefault(s => s.UserId == userId);

            int studentId = student.StudentId;         

            var isStudentEnrolled = await _enrollmentRepository.Get();
            var studentEnrolled = isStudentEnrolled.FirstOrDefault(enrollment => enrollment.StudentId == studentId && enrollment.CourseCode == isAssignmentExists.CourseCode);

            if (studentEnrolled == null)
                throw new NotEnrolledInCourseException();

            Submission submission = new Submission();
            submission.StudentId = student.StudentId;
            submission.SubmissionDate = DateTime.Now;
            submission.IsCompleted = assignmentSubmission.IsCompleted;

            var createdSubmission = await _sumissionRepository.Add(submission);

            return MapSubmissionToSubmissionDTO(createdSubmission);
        }

        public AssignmentSubmisssionReturnDTO MapSubmissionToSubmissionDTO(Submission submission)
        {
            AssignmentSubmisssionReturnDTO assignmentSubmisssionReturnDTO = new AssignmentSubmisssionReturnDTO();
            assignmentSubmisssionReturnDTO.SubmissionDate = submission.SubmissionDate;
            assignmentSubmisssionReturnDTO.stautus = submission.IsCompleted ? "Completed" : "Not Completed";

            return assignmentSubmisssionReturnDTO;
        }

        public async Task<IEnumerable<AssignmentDTO>> GetAssignedAssignments(int userId)
        {

            var studentDb = await _studentRepository.Get();
            var student = studentDb.FirstOrDefault(s => s.UserId == userId);

            int studentId = student.StudentId;


            var enrollments = await _enrollmentRepository.Get();
            var courses = enrollments.Where(enrollment => enrollment.StudentId == studentId)
                                     .Select(enrollment => enrollment.CourseCode)
                                     .ToList();

            if (enrollments.Count() == 0)
                throw new NotEnrolledInAnyCourseException();

            var assignments = await _assignmentRepository.Get();
            var assignedAssignments = assignments.Where(assignment => courses.Contains(assignment.CourseCode));

            if (!assignedAssignments.Any())
                throw new NoAssignmentFoundException();

            return assignedAssignments.Select(MapAssignmentToAssignmentDTO);
        }


        public async Task<IEnumerable<AssignmentDTO>> GetAssignedAssignmentsByCourse(int userId, string courseCode)
        {
            var course = await _courseRepository.Get(courseCode);
            if (course == null)
                throw new NoSuchCourseException();

            var studentDb = await _studentRepository.Get();
            var student = studentDb.FirstOrDefault(s => s.UserId == userId);

            int studentId = student.StudentId;


            var enrollments = await _enrollmentRepository.Get();
            var courses = enrollments.Where(enrollment => enrollment.StudentId == studentId && enrollment.CourseCode == courseCode)
                         .ToList();

            if (!courses.Any())
                throw new NotEnrolledInCourseException();

            var assignments = await _assignmentRepository.Get();
            var assignedAssignments = assignments.Where(assignment => assignment.CourseCode == courseCode)
                                     .ToList();

            if (!assignedAssignments.Any())
                throw new NoAssignmentFoundException();

            return assignedAssignments.Select(MapAssignmentToAssignmentDTO);
        }

        public Task<AssignmentDTO> GetAssignmentSubmissionStatus(int userId, int assignmentId)
        {
            throw new NotImplementedException();
        }

        public AssignmentDTO MapAssignmentToAssignmentDTO(Assignment assignment)
        {
            AssignmentDTO assignmentDTO = new AssignmentDTO();
            assignmentDTO.AssignmentId = assignment.AssignmentId;
            assignmentDTO.Title = assignment.Title;
            assignmentDTO.AssignmentDueDate = assignment.DueDate;
            assignmentDTO.CourseCode = assignment.CourseCode;
            return assignmentDTO;
        }
    }
}
