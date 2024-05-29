using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Interfaces
{
    public interface ITeacherService
    {
        public Task<TeacherReturnDTO> RegisterTeacher(TeacherRegisterDTO teacher);
        public Task<TeacherReturnDTO> UpdateTeacherEmail(UpdateEmailDTO updateEmaildto);
        public Task<TeacherReturnDTO> UpdateTeacherPhone(UpdatePhoneDTO updatePhonedto);
        public Task<TeacherReturnDTO> GetTeacherById(int teacherId);
        public Task<IEnumerable<TeacherReturnDTO>> GetTeachers();
        public Task<TeacherReturnDTO> DeleteTeacher(int teacherId);
    }
}
