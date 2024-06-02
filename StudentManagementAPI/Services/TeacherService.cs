using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagementAPI.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IRepository<int, Teacher> _teacherRepository;
        private readonly ILogger<TeacherService> _logger;

        public TeacherService(IRepository<int, Teacher> teacherRepository, ILogger<TeacherService> logger)
        {
            _teacherRepository = teacherRepository;
            _logger = logger;
        }

        public async Task<TeacherReturnDTO> RegisterTeacher(TeacherRegisterDTO teacher)
        {
            try
            {
                var newTeacher = new Teacher
                {
                    FullName = teacher.FullName,
                    DateOfBirth = teacher.DateOfBirth,
                    Gender = teacher.Gender,
                    Phone = teacher.Phone,
                    Email = teacher.Email
                };

                await _teacherRepository.Add(newTeacher);

                return MapTeacherToTeacherReturnDTO(newTeacher);
            }
            catch (UnableToAddException ex)
            {
                _logger.LogError(ex, "Unable to Register Teacher");
                throw new UnableToAddException("Unable to Register Teacher. Please check the data and try again.");
            }
        }

        public async Task<TeacherReturnDTO> UpdateTeacherEmail(UpdateEmailDTO updateEmailDto)
        {
            var teacher = await GetTeacherByIdOrThrow(updateEmailDto.Id);
            teacher.Email = updateEmailDto.Email;
            await _teacherRepository.Update(teacher);

            return MapTeacherToTeacherReturnDTO(teacher);
        }

        public async Task<TeacherReturnDTO> UpdateTeacherPhone(UpdatePhoneDTO updatePhoneDto)
        {
            var teacher = await GetTeacherByIdOrThrow(updatePhoneDto.Id);
            teacher.Phone = updatePhoneDto.Phone;
            await _teacherRepository.Update(teacher);

            return MapTeacherToTeacherReturnDTO(teacher);
        }

        public async Task<TeacherReturnDTO> GetTeacherById(int key)
        {
            var teacher = await GetTeacherByIdOrThrow(key);
            return MapTeacherToTeacherReturnDTO(teacher);
        }

        public async Task<IEnumerable<TeacherReturnDTO>> GetTeachers()
        {
            var teachers = await _teacherRepository.Get();
            if (!teachers.Any())
            {
                _logger.LogWarning("No teachers found");
                throw new NoTeacherFoundException();
            }

            return teachers.Select(MapTeacherToTeacherReturnDTO).ToList();
        }

        public async Task<TeacherReturnDTO> DeleteTeacher(int teacherId)
        {
            var teacher = await GetTeacherByIdOrThrow(teacherId);
            var deletedTeacher = await _teacherRepository.Delete(teacherId);
            return MapTeacherToTeacherReturnDTO(deletedTeacher);
        }

        private async Task<Teacher> GetTeacherByIdOrThrow(int id)
        {
            var teacher = await _teacherRepository.Get(id);
            if (teacher == null)
            {
                _logger.LogWarning("Teacher not found with ID: {Id}", id);
                throw new NoSuchTeacherException();
            }

            return teacher;
        }

        private TeacherReturnDTO MapTeacherToTeacherReturnDTO(Teacher teacher)
        {
            return new TeacherReturnDTO
            {
                TeacherId = teacher.TeacherId,
                FullName = teacher.FullName,
                DateOfBirth = teacher.DateOfBirth,
                Gender = teacher.Gender,
                Phone = teacher.Phone,
                Email = teacher.Email
            };
        }
    }
}
