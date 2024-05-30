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

            if (student == null)
                throw new NoLinkedAccountException();

            int studentId = student.StudentId;

            var isStudentEnrolled = await _enrollmentRepository.Get();
            var studentEnrolled = isStudentEnrolled.FirstOrDefault(enrollment => enrollment.StudentId == studentId && enrollment.CourseCode == isAssignmentExists.CourseCode);

            var submissionsDB = await _sumissionRepository.Get();
            var isAlreadySubmitted = submissionsDB.FirstOrDefault(s => s.StudentId == studentId && s.AssignmentId == assignmentSubmission.AssignmentId);

            if (isAlreadySubmitted != null)
                throw new DuplicateAssignmentSubmissionException();

            if (studentEnrolled == null)
                throw new NotEnrolledInCourseException();


            bool isCorrectFileExt = Path.GetExtension(assignmentSubmission.File.FileName).ToLower() == ".pdf";
            if (!isCorrectFileExt)
                throw new InvalidFileExtensionException();

            Submission submission = new Submission
            {
                AssignmentId = assignmentSubmission.AssignmentId,
                StudentId = student.StudentId,
                SubmissionDate = DateTime.Now
            };

            string sanitizedFileName = GenerateFileName(assignmentSubmission, studentId);

            if (assignmentSubmission.File != null && assignmentSubmission.File.Length > 0)
            {
                var filePath = Path.Combine("SubmittedAssignments", sanitizedFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await assignmentSubmission.File.CopyToAsync(stream);
                }
                submission.FileName = sanitizedFileName;
            }

            var createdSubmission = await _sumissionRepository.Add(submission);
            return MapSubmissionToSubmissionDTO(createdSubmission);
        }

        public string GenerateFileName(AssignmentSubmisssionDTO assignmentSubmission, int studentID)
        {
            string assignmentIdStr = assignmentSubmission.AssignmentId.ToString();
            string studentIdStr = studentID.ToString();
            string currentDate = DateTime.Now.ToString("yyyyMMddHHmmss");

            string sanitizedFileName = string.Join("_", assignmentIdStr, studentIdStr, currentDate, assignmentSubmission.File.FileName)
                .Replace(":", "_")
                .Replace(" ", "_");

            return sanitizedFileName;
        }

        public AssignmentSubmisssionReturnDTO MapSubmissionToSubmissionDTO(Submission submission)
        {
            AssignmentSubmisssionReturnDTO assignmentSubmisssionReturnDTO = new AssignmentSubmisssionReturnDTO
            {
                AssignmentId = submission.AssignmentId,
                SubmissionDate = submission.SubmissionDate,
                FileName = submission.FileName
            };

            return assignmentSubmisssionReturnDTO;
        }

        public async Task<IEnumerable<AssignmentDTO>> GetAssignedAssignments(int userId)
        {

            var studentDb = await _studentRepository.Get();
            var student = studentDb.FirstOrDefault(s => s.UserId == userId);

            if (student == null)
                throw new NoLinkedAccountException();

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

            if (student == null)
                throw new NoLinkedAccountException();

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

        public async Task<AssignmentSubmisssionReturnDTO> GetAssignmentSubmissionStatus(int userId, int assignmentId)
        {
            var studentDb = await _studentRepository.Get();
            var student = studentDb.FirstOrDefault(s => s.UserId == userId);

            if (student == null)
                throw new NoLinkedAccountException();

            int studentId = student.StudentId;

            var submissionDb = await _sumissionRepository.Get();
            var submission = submissionDb.FirstOrDefault(s => s.StudentId == studentId && s.AssignmentId == assignmentId);

            if (submission == null)
                throw new NoSuchAssignmentSubmissionException();

            return MapSubmissionToSubmissionDTO(submission);
        }

        public AssignmentDTO MapAssignmentToAssignmentDTO(Assignment assignment)
        {
            AssignmentDTO assignmentDTO = new AssignmentDTO
            {
                AssignmentId = assignment.AssignmentId,
                Title = assignment.Title,
                AssignmentDueDate = assignment.DueDate,
                CourseCode = assignment.CourseCode
            };
            return assignmentDTO;
        }

        public async Task<AssignmentSubmissionResultDTO> GetSubmittedAssignment(int assignmentId, int StudentId)
        {
            var assignment = await _assignmentRepository.Get(assignmentId);
            if (assignment == null)
                throw new NoSuchAssignmentException();

            var student = await _studentRepository.Get(StudentId);
            if (student == null)
                throw new NoSuchStudentException();

            var submission = await _sumissionRepository.Get();
            var assignmentSubmission = submission.FirstOrDefault(s => s.AssignmentId == assignmentId && s.StudentId == StudentId);

            if (assignmentSubmission == null)
                throw new NoSuchAssignmentSubmissionException();

            var filePath = Path.Combine("SubmittedAssignments", assignmentSubmission.FileName);
            var fileData = await File.ReadAllBytesAsync(filePath);

            return new AssignmentSubmissionResultDTO
            {
                FileData = fileData,
                FileName = assignmentSubmission.FileName
            };
        }
    }
}
