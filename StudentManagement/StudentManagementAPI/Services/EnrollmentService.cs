using Microsoft.AspNetCore.Authorization;
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

        public EnrollmentService(IRepository<int, Enrollment> enrollmentRepository, IRepository<int, Student> studentRepository, IRepository<string, Course> courseRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }

        public async Task<EnrollmentReturnDTO> EnrollStudent(int StudentId, string CourseCode)
        {
            var student = await _studentRepository.Get(StudentId);
            var course = await _courseRepository.Get(CourseCode);

            if (student == null)
                throw new NoSuchStudentException();

            if (course == null)
                throw new NoSuchCourseException();

            var enrollments = await _enrollmentRepository.Get();
            var enrollment = enrollments.FirstOrDefault(enrollment => enrollment.StudentId == StudentId && enrollment.CourseCode == CourseCode);

            if (enrollment != null)
                throw new StudentAlreadyEnrolledException();

            Enrollment newEnrollment = new Enrollment();
            newEnrollment.StudentId = StudentId;
            newEnrollment.CourseCode = CourseCode;

            newEnrollment = await _enrollmentRepository.Add(newEnrollment);
            return MapEnrollmentToEnrollmentReturnDTO(newEnrollment);
        }

        public async Task<IEnumerable<EnrollmentReturnDTO>> GetEnrollmentsByCourseId(string CourseCode)
        {
            var enrollments = await _enrollmentRepository.Get();
            enrollments = enrollments.Where(enrollment => enrollment.CourseCode == CourseCode);

            if (enrollments.Count() == 0)
                throw new NoSuchEnrollmentException();

            var enrollmentsDTOs = new List<EnrollmentReturnDTO>();
            foreach (var enrollment in enrollments)
                enrollmentsDTOs.Add(MapEnrollmentToEnrollmentReturnDTO(enrollment));

            return enrollmentsDTOs;
        }

        public async Task<IEnumerable<EnrollmentReturnDTO>> GetEnrollmentsByStudentId(int StudentId)
        {
            var enrollments = await _enrollmentRepository.Get();
            enrollments = enrollments.Where(enrollment => enrollment.StudentId == StudentId);

            if (enrollments.Count() == 0)
                throw new NoSuchEnrollmentException();

            var enrollmentsDTOs = new List<EnrollmentReturnDTO>();
            foreach (var enrollment in enrollments)
                enrollmentsDTOs.Add(MapEnrollmentToEnrollmentReturnDTO(enrollment));

            return enrollmentsDTOs;
        }

        public async Task<EnrollmentReturnDTO> UnenrollStudent(int StudentId, string CourseCode)
        {
            var student = await _studentRepository.Get(StudentId);
            var course = await _courseRepository.Get(CourseCode);
            
            if (student == null)
                throw new NoSuchStudentException();

            if (course == null)
                throw new NoSuchCourseException();

            var enrollments = await _enrollmentRepository.Get();
            var enrollment = enrollments.FirstOrDefault(enrollment => enrollment.StudentId == StudentId && enrollment.CourseCode == CourseCode);

            await _enrollmentRepository.Delete(enrollment.EnrollmentId);
            return MapEnrollmentToEnrollmentReturnDTO(enrollment);
        }

        public async Task<IEnumerable<EnrollmentReturnDTO>> GetAllEnrollments()
        {
            var enrollments = await _enrollmentRepository.Get();
            var enrollmentsDTOs = new List<EnrollmentReturnDTO>();
            foreach (var enrollment in enrollments)
                enrollmentsDTOs.Add(MapEnrollmentToEnrollmentReturnDTO(enrollment));

            return enrollmentsDTOs;
        }


        public EnrollmentReturnDTO MapEnrollmentToEnrollmentReturnDTO(Enrollment enrollment)
        {
            EnrollmentReturnDTO newEnrollment = new EnrollmentReturnDTO();
            newEnrollment.EnrollmentId = enrollment.EnrollmentId;
            newEnrollment.StudentId = enrollment.StudentId;
            newEnrollment.CourseCode = enrollment.CourseCode;
            newEnrollment.EnrollmentDate = enrollment.EnrollmentDate;
            return newEnrollment;
        }
    }
}
