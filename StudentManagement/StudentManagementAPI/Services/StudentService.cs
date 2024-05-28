using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<int, Student> _studentRepository;

        public StudentService(IRepository<int, Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<StudentReturnDTO> RegisterStudent(StudentRegisterDTO student)
        {
            var newStudent = new Student();
            newStudent.FullName = student.FullName;
            newStudent.RollNo = student.RollNo;
            newStudent.Gender = student.Gender;
            newStudent.Department = student.Department;
            newStudent.DateOfBirth = student.DateOfBirth;
            newStudent.Phone = student.Phone;
            newStudent.Email = student.Email;
            newStudent.Status = student.Status;

            var AddedStudent = await _studentRepository.Add(newStudent);

            return MapStudentToStudentReturnDTO(AddedStudent);

        }


        public async Task<StudentReturnDTO> UpdateStudentEmail(int studentId, string email)
        {
            var student = await _studentRepository.Get(studentId);
            if (student != null)
            {
                student.Email = email;
                var updatedStudent = await _studentRepository.Update(student);
                return MapStudentToStudentReturnDTO(updatedStudent);
            }

            throw new NoSuchStudentException();
        }


        public async Task<StudentReturnDTO> UpdateStudentPhone(int studentId, string phone)
        {
            var student = await _studentRepository.Get(studentId);
            if (student != null)
            {
                student.Phone = phone;
                var updatedStudent = await _studentRepository.Update(student);
                return MapStudentToStudentReturnDTO(updatedStudent);
            }
            throw new NoSuchStudentException();
        }

        public async Task<StudentReturnDTO> UpdateStudentStatus(int studentId, string status)
        {
            var student = await _studentRepository.Get(studentId);
            if (student != null)
            {
                student.Status = status;
                var updatedStudent = await _studentRepository.Update(student);
                return MapStudentToStudentReturnDTO(updatedStudent);
            }
            throw new NoSuchStudentException();
        }


        public async Task<StudentReturnDTO> GetStudentById(int studentId)
        {
            var student = await _studentRepository.Get(studentId);
            if (student == null)
                throw new NoSuchStudentException();
            return MapStudentToStudentReturnDTO(student);
        }


        public async Task<IEnumerable<StudentReturnDTO>> GetStudents()
        {
            var students = await _studentRepository.Get();

            if (students.Count() == 0)
                throw new NoStudentFoundException();

            var studentDTOs = new List<StudentReturnDTO>();
            foreach (var student in students)
                studentDTOs.Add(MapStudentToStudentReturnDTO(student));
            return studentDTOs;
        }


        public async Task<StudentReturnDTO> DeleteStudent(int studentId)
        {
            try
            {
                var student = await _studentRepository.Delete(studentId);
                return MapStudentToStudentReturnDTO(student);
            }

            catch (NoSuchStudentException)
            {
                throw new NoSuchStudentException();
            }
        }


        public StudentReturnDTO MapStudentToStudentReturnDTO(Student student)
        {
            StudentReturnDTO studentReturnDTO = new StudentReturnDTO();
            studentReturnDTO.StudentId = student.StudentId;
            studentReturnDTO.FullName = student.FullName;
            studentReturnDTO.RollNo = student.RollNo;
            studentReturnDTO.Gender = student.Gender;
            studentReturnDTO.Department = student.Department;
            studentReturnDTO.DateOfBirth = student.DateOfBirth;
            studentReturnDTO.Phone = student.Phone;
            studentReturnDTO.Email = student.Email;
            studentReturnDTO.Status = student.Status;

            return studentReturnDTO;
        }
    }
}