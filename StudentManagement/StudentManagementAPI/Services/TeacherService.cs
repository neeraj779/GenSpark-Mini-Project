using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IRepository<int, Teacher> _teacherRepository;

        public TeacherService(IRepository<int, Teacher> teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }


        public async Task<TeacherReturnDTO> DeleteTeacher(int teacherId)
        {
            var teacher = await _teacherRepository.Delete(teacherId);
            return MapTeacherToTeacherReturnDTO(teacher);
        }

        public async Task<TeacherReturnDTO> UpdateTeacher(Teacher teacher)
        {
            var updatedTeacher = await _teacherRepository.Update(teacher);
            return MapTeacherToTeacherReturnDTO(updatedTeacher);
        }

        public async Task<TeacherReturnDTO> GetTeacherById(int key)
        {
            var teacher = await _teacherRepository.Get(key);
            return MapTeacherToTeacherReturnDTO(teacher);
        }

        public async Task<IEnumerable<TeacherReturnDTO>> GetTeachers()
        {
            var teachers = await _teacherRepository.Get();
            var teacherDTOs = new List<TeacherReturnDTO>();
            foreach (var teacher in teachers)
                teacherDTOs.Add(MapTeacherToTeacherReturnDTO(teacher));
            return teacherDTOs;
        }

        public async Task<TeacherRegisterDTO> RegisterTeacher(TeacherRegisterDTO teacher)
        {
            Teacher newTeacher = new Teacher();
            newTeacher.FullName = teacher.FullName;
            newTeacher.DateOfBirth = teacher.DateOfBirth;
            newTeacher.Gender = teacher.Gender;
            newTeacher.Phone = teacher.Phone;
            newTeacher.Email = teacher.Email;

            await _teacherRepository.Add(newTeacher);

            return MapTeacherToTeacherRegisterDTO(newTeacher);
        }

        private TeacherRegisterDTO MapTeacherToTeacherRegisterDTO(Teacher newTeacher)
        {
            TeacherRegisterDTO teacherRegisterDTO = new TeacherRegisterDTO();
            teacherRegisterDTO.FullName = newTeacher.FullName;
            teacherRegisterDTO.DateOfBirth = newTeacher.DateOfBirth;
            teacherRegisterDTO.Gender = newTeacher.Gender;
            teacherRegisterDTO.Phone = newTeacher.Phone;
            teacherRegisterDTO.Email = newTeacher.Email;

            return teacherRegisterDTO;
        }

        private TeacherReturnDTO MapTeacherToTeacherReturnDTO(Teacher newTeacher)
        {
            TeacherReturnDTO teacherReturnDTO = new TeacherReturnDTO();
            teacherReturnDTO.TeacherId = newTeacher.TeacherId;
            teacherReturnDTO.FullName = newTeacher.FullName;
            teacherReturnDTO.DateOfBirth = newTeacher.DateOfBirth;
            teacherReturnDTO.Gender = newTeacher.Gender;
            teacherReturnDTO.Phone = newTeacher.Phone;
            teacherReturnDTO.Email = newTeacher.Email;

            return teacherReturnDTO;
        }
    }

}
