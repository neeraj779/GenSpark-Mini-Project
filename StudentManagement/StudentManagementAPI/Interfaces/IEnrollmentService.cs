using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Interfaces
{
    public interface IEnrollmentService
    {
        public Task<EnrollmentReturnDTO> EnrollStudent(int StudentId, string CourseCode);
        public Task<EnrollmentReturnDTO> UnenrollStudent(int StudentId, string CourseCode);
        public Task<IEnumerable<EnrollmentReturnDTO>> GetEnrollmentsByStudentId(int StudentId);
        public Task<IEnumerable<EnrollmentReturnDTO>> GetEnrollmentsByCourseId(string CourseCode);
        public Task<IEnumerable<EnrollmentReturnDTO>> GetAllEnrollments();
    }
}
