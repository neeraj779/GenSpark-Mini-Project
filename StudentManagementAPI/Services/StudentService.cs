using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<int, Student> _studentRepository;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IRepository<int, Student> studentRepository, ILogger<StudentService> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        public async Task<StudentReturnDTO> RegisterStudent(StudentRegisterDTO student)
        {
            if (!Enum.TryParse(student.Status, out StudentStatus status))
            {
                _logger.LogWarning("Invalid student status: {Status}", student.Status);
                throw new InvalidStudentStatusException();
            }

            var newStudent = new Student
            {
                FullName = student.FullName,
                RollNo = student.RollNo,
                Gender = student.Gender,
                Department = student.Department,
                DateOfBirth = student.DateOfBirth,
                Phone = student.Phone,
                Email = student.Email,
                Status = status
            };

            try
            {
                var addedStudent = await _studentRepository.Add(newStudent);
                return MapStudentToStudentReturnDTO(addedStudent);
            }
            catch (UnableToAddException ex)
            {
                _logger.LogError(ex, "Unable to register student: {Student}", student);
                throw new UnableToAddException("Unable to register student. Please check the data and try again.");
            }
        }

        public async Task<StudentReturnDTO> UpdateStudentEmail(UpdateEmailDTO updateEmailDto)
        {
            var student = await GetStudentByIdOrThrow(updateEmailDto.Id);
            student.Email = updateEmailDto.Email;
            var updatedStudent = await _studentRepository.Update(student);
            return MapStudentToStudentReturnDTO(updatedStudent);
        }

        public async Task<StudentReturnDTO> UpdateStudentPhone(UpdatePhoneDTO updatePhoneDto)
        {
            var student = await GetStudentByIdOrThrow(updatePhoneDto.Id);
            student.Phone = updatePhoneDto.Phone;
            var updatedStudent = await _studentRepository.Update(student);
            return MapStudentToStudentReturnDTO(updatedStudent);
        }

        public async Task<StudentReturnDTO> UpdateStudentStatus(UpdateStatusDTO updateStatusdto)
        {
            var student = await GetStudentByIdOrThrow(updateStatusdto.StudentId);

            if (!Enum.TryParse(updateStatusdto.Status, out StudentStatus studentStatus))
            {
                _logger.LogWarning("Invalid student status: {Status}", updateStatusdto.Status);
                throw new InvalidStudentStatusException();
            }

            student.Status = studentStatus;
            var updatedStudent = await _studentRepository.Update(student);
            return MapStudentToStudentReturnDTO(updatedStudent);
        }

        public async Task<StudentReturnDTO> GetStudentById(int studentId)
        {
            var student = await GetStudentByIdOrThrow(studentId);
            return MapStudentToStudentReturnDTO(student);
        }

        public async Task<IEnumerable<StudentReturnDTO>> GetStudents()
        {
            var students = await _studentRepository.Get();
            if (!students.Any())
            {
                _logger.LogWarning("No students found");
                throw new NoStudentFoundException();
            }

            return students.Select(MapStudentToStudentReturnDTO).ToList();
        }

        public async Task<StudentReturnDTO> DeleteStudent(int studentId)
        {
            await GetStudentByIdOrThrow(studentId);
            var deletedStudent = await _studentRepository.Delete(studentId);
            return MapStudentToStudentReturnDTO(deletedStudent);
        }

        private async Task<Student> GetStudentByIdOrThrow(int studentId)
        {
            var student = await _studentRepository.Get(studentId);
            if (student == null)
            {
                _logger.LogWarning("Student not found with ID: {Id}", studentId);
                throw new NoSuchStudentException();
            }
            return student;
        }

        private StudentReturnDTO MapStudentToStudentReturnDTO(Student student)
        {
            return new StudentReturnDTO
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                RollNo = student.RollNo,
                Gender = student.Gender,
                Department = student.Department,
                DateOfBirth = student.DateOfBirth,
                Phone = student.Phone,
                Email = student.Email,
                Status = student.Status.ToString()
            };
        }
    }
}
