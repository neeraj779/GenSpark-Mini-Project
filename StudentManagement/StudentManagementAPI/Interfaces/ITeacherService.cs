using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Interfaces
{
    public interface ITeacherService
    {
        public Task<TeacherRegisterDTO> RegisterTeacher(TeacherRegisterDTO teacher);
        public Task<TeacherReturnDTO> DeleteTeacher(int teacherId);
        public Task<TeacherReturnDTO> GetTeacherById(int teacherId);
        public Task<IEnumerable<TeacherReturnDTO>> GetTeachers();
    }
}
