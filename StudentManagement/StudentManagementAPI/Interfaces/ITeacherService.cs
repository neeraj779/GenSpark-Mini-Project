using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Interfaces
{
    public interface ITeacherService
    {
        public Task<TeacherReturnDTO> RegisterTeacher(TeacherRegisterDTO teacher);
        public Task<TeacherReturnDTO> UpdateTeacherEmail(int teacherId, string email);
        public Task<TeacherReturnDTO> UpdateTeacherPhone(int teacherId, string phone);
        public Task<TeacherReturnDTO> GetTeacherById(int teacherId);
        public Task<IEnumerable<TeacherReturnDTO>> GetTeachers();
        public Task<TeacherReturnDTO> DeleteTeacher(int teacherId);
    }
}
