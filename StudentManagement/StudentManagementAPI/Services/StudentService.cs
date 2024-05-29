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

            if (!Enum.TryParse(student.Status, out StudentStatus status))
            {
                throw new InvalidStudentStatusException();
            }

            var newStudent = new Student();
            newStudent.FullName = student.FullName;
            newStudent.RollNo = student.RollNo;
            newStudent.Gender = student.Gender;
            newStudent.Department = student.Department;
            newStudent.DateOfBirth = student.DateOfBirth;
            newStudent.Phone = student.Phone;
            newStudent.Email = student.Email;
            newStudent.Status = status;

            try
            {
                var AddedStudent = await _studentRepository.Add(newStudent);
                return MapStudentToStudentReturnDTO(AddedStudent);
            }

            catch (UnableToAddException)
            {
                throw new UnableToAddException("Unable to Register Student. Please check the data and try again.");
            }
        }


        public async Task<StudentReturnDTO> UpdateStudentEmail(UpdateEmailDTO updateEmaildto)
        {
            var student = await _studentRepository.Get(updateEmaildto.Id);
            if (student != null)
            {
                student.Email = updateEmaildto.Email;
                var updatedStudent = await _studentRepository.Update(student);
                return MapStudentToStudentReturnDTO(updatedStudent);
            }

            throw new NoSuchStudentException();
        }


        public async Task<StudentReturnDTO> UpdateStudentPhone(UpdatePhoneDTO updatePhonedto)
        {
            var student = await _studentRepository.Get(updatePhonedto.Id);
            if (student != null)
            {
                student.Phone = updatePhonedto.Phone;
                var updatedStudent = await _studentRepository.Update(student);
                return MapStudentToStudentReturnDTO(updatedStudent);
            }
            throw new NoSuchStudentException();
        }

        public async Task<StudentReturnDTO> UpdateStudentStatus(int studentId, string status)
        {
            var student = await _studentRepository.Get(studentId);

            if (!Enum.TryParse(status, out StudentStatus studentStatus))
            {
                throw new InvalidStudentStatusException();
            }


            if (student != null)
            {
                student.Status = studentStatus;
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
            studentReturnDTO.Status = student.Status.ToString();

            return studentReturnDTO;
        }
    }
}