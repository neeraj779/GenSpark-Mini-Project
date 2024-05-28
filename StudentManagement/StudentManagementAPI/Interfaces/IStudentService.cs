using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Interfaces
{
    public interface IStudentService
    {
        public Task<StudentReturnDTO> RegisterStudent(StudentRegisterDTO student);
        public Task<StudentReturnDTO> GetStudentById(int studentId);
        public Task<IEnumerable<StudentReturnDTO>> GetStudents();
        public Task<StudentReturnDTO> DeleteStudent(int studentId);
    }
}
