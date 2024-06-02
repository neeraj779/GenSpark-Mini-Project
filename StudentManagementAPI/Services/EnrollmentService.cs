using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IRepository<int, Enrollment> _enrollmentRepository;
        private readonly IRepository<int, Student> _studentRepository;
        private readonly IRepository<string, Course> _courseRepository;
        private readonly ILogger<EnrollmentService> _logger;

        public EnrollmentService(
            IRepository<int, Enrollment> enrollmentRepository,
            IRepository<int, Student> studentRepository,
            IRepository<string, Course> courseRepository,
            ILogger<EnrollmentService> logger)
        {
            _enrollmentRepository = enrollmentRepository;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _logger = logger;
        }

        public async Task<EnrollmentReturnDTO> EnrollStudent(int studentId, string courseCode)
        {
            var student = await EnsureStudentExists(studentId);
            var course = await EnsureCourseExists(courseCode);

            var existingEnrollments = await _enrollmentRepository.Get();
            if (existingEnrollments.Any(e => e.StudentId == studentId && e.CourseCode == courseCode))
            {
                _logger.LogWarning("Student {StudentId} is already enrolled in course {CourseCode}", studentId, courseCode);
                throw new StudentAlreadyEnrolledException();
            }

            var newEnrollment = new Enrollment
            {
                StudentId = studentId,
                CourseCode = courseCode
            };

            var createdEnrollment = await _enrollmentRepository.Add(newEnrollment);
            return MapToDTO(createdEnrollment);
        }

        public async Task<IEnumerable<EnrollmentReturnDTO>> GetEnrollmentsByCourseCode(string courseCode)
        {
            await EnsureCourseExists(courseCode);

            var enrollments = await _enrollmentRepository.Get();
            var courseEnrollments = enrollments.Where(e => e.CourseCode == courseCode).ToList();

            if (!courseEnrollments.Any())
            {
                _logger.LogWarning("No enrollments found for course {CourseCode}", courseCode);
                throw new NoSuchEnrollmentException();
            }
            return courseEnrollments.Select(MapToDTO).ToList();
        }

        public async Task<IEnumerable<EnrollmentReturnDTO>> GetEnrollmentsByStudentId(int studentId)
        {
            await EnsureStudentExists(studentId);

            var enrollments = await _enrollmentRepository.Get();
            var studentEnrollments = enrollments.Where(e => e.StudentId == studentId).ToList();

            if (!studentEnrollments.Any())
            {
                _logger.LogWarning("No enrollments found for student {StudentId}", studentId);
                throw new NoSuchEnrollmentException();
            }
            return studentEnrollments.Select(MapToDTO).ToList();
        }

        public async Task<EnrollmentReturnDTO> UnenrollStudent(int studentId, string courseCode)
        {
            var student = await EnsureStudentExists(studentId);
            var course = await EnsureCourseExists(courseCode);

            var enrollments = await _enrollmentRepository.Get();
            var enrollment = enrollments.FirstOrDefault(e => e.StudentId == studentId && e.CourseCode == courseCode);

            if (enrollment == null)
            {
                _logger.LogWarning("Enrollment not found for student {StudentId} in course {CourseCode}", studentId, courseCode);
                throw new NoSuchEnrollmentException();
            }

            await _enrollmentRepository.Delete(enrollment.EnrollmentId);
            return MapToDTO(enrollment);
        }

        public async Task<IEnumerable<EnrollmentReturnDTO>> GetAllEnrollments()
        {
            var enrollments = await _enrollmentRepository.Get();

            if (!enrollments.Any())
            {
                _logger.LogWarning("No enrollments found");
                throw new NoEnrollmentFoundException();
            }

            return enrollments.Select(MapToDTO).ToList();
        }

        private async Task<Student> EnsureStudentExists(int studentId)
        {
            var student = await _studentRepository.Get(studentId);
            if (student == null)
            {
                _logger.LogWarning("Student not found with ID: {StudentId}", studentId);
                throw new NoSuchStudentException();
            }
            return student;
        }

        private async Task<Course> EnsureCourseExists(string courseCode)
        {
            var course = await _courseRepository.Get(courseCode);
            if (course == null)
            {
                _logger.LogWarning("Course not found with code: {CourseCode}", courseCode);
                throw new NoSuchCourseException();
            }
            return course;
        }

        private EnrollmentReturnDTO MapToDTO(Enrollment enrollment)
        {
            return new EnrollmentReturnDTO
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = enrollment.StudentId,
                CourseCode = enrollment.CourseCode,
                EnrollmentDate = enrollment.EnrollmentDate
            };
        }
    }
}
