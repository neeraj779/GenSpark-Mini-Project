using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;
using StudentManagementAPI.Repositories;

namespace StudentManagementAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<int, Student> _studentRepository;

        public StudentService(IRepository<int, Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<StudentReturnDTO> DeleteStudent(int studentId)
        {
            var student = await _studentRepository.Delete(studentId);
            return MapStudentToStudentReturnDTO(student);
        }

        public async Task<StudentReturnDTO> GetStudentById(int studentId)
        {
            var student = await _studentRepository.Get(studentId);
            return MapStudentToStudentReturnDTO(student);
        }

        public async Task<IEnumerable<StudentReturnDTO>> GetStudents()
        {
            var students = await _studentRepository.Get();
            var studentDTOs = new List<StudentReturnDTO>();
            foreach (var student in students)
                studentDTOs.Add(MapStudentToStudentReturnDTO(student));
            return studentDTOs;
        }

        public async Task<StudentReturnDTO> RegisterStudent(StudentRegisterDTO student)
        {
            var newStudent = new Student();
            newStudent.FullName = student.FullName;
            newStudent.RollNo = student.RollNo;
            newStudent.Department = student.Department;
            newStudent.DateOfBirth = student.DateOfBirth;
            newStudent.Phone = student.Phone;
            newStudent.Email = student.Email;
            newStudent.Status = student.Status;

            var AddedStudent = await _studentRepository.Add(newStudent);

            return MapStudentToStudentReturnDTO(AddedStudent);

        }


        public StudentReturnDTO MapStudentToStudentReturnDTO(Student student)
        {
            StudentReturnDTO studentReturnDTO = new StudentReturnDTO();
            studentReturnDTO.StudentId = student.StudentId;
            studentReturnDTO.FullName = student.FullName;
            studentReturnDTO.RollNo = student.RollNo;
            studentReturnDTO.Department = student.Department;
            studentReturnDTO.DateOfBirth = student.DateOfBirth;
            studentReturnDTO.Phone = student.Phone;
            studentReturnDTO.Email = student.Email;
            studentReturnDTO.Status = student.Status;

            return studentReturnDTO;
        }
    }
}