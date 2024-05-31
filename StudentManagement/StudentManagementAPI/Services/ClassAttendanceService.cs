using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Services
{
    public class ClassAttendanceService : IClassAttendanceService
    {
        private readonly IRepository<int, ClassAttendance> _classAttendanceRepository;
        private readonly IRepository<int, Class> _classRepository;
        private readonly IRepository<int, Student> _studentRepository;

        public ClassAttendanceService(
            IRepository<int, ClassAttendance> classAttendanceRepository,
            IRepository<int, Class> classRepository,
            IRepository<int, Student> studentRepository)
        {
            _classAttendanceRepository = classAttendanceRepository;
            _classRepository = classRepository;
            _studentRepository = studentRepository;
        }

        public async Task<ClassAttendanceReturnDTO> MarkStudentAttendance(ClassAttendanceDTO classAttendanceDto)
        {
            await ValidateClassAndStudentExistence(classAttendanceDto.ClassId, classAttendanceDto.StudentId);

            if (!Enum.TryParse(classAttendanceDto.Status, out AttendanceStatus status))
                throw new InvalidAttendanceStatusException();

            var existingAttendance = (await _classAttendanceRepository.Get())
                .FirstOrDefault(x => x.ClassId == classAttendanceDto.ClassId && x.StudentId == classAttendanceDto.StudentId);

            if (existingAttendance != null)
                throw new ClassAttendanceAlreadyExistsException();

            var classAttendance = new ClassAttendance
            {
                ClassId = classAttendanceDto.ClassId,
                StudentId = classAttendanceDto.StudentId,
                Date = DateTime.Now,
                Status = status
            };

            var newAttendance = await _classAttendanceRepository.Add(classAttendance);
            return MapClassAttendanceToDTO(newAttendance);
        }

        public async Task<IEnumerable<ClassAttendanceReturnDTO>> GetAttendanceByClass(int classId)
        {
            await ValidateClassExistence(classId);

            var classAttendances = (await _classAttendanceRepository.Get())
                .Where(x => x.ClassId == classId);

            if (!classAttendances.Any())
                throw new NoClassAttendanceFoundException();

            return classAttendances.Select(MapClassAttendanceToDTO);
        }

        public async Task<IEnumerable<ClassAttendanceReturnDTO>> GetAttendanceByStudent(int studentId)
        {
            await ValidateStudentExistence(studentId);

            var classAttendances = (await _classAttendanceRepository.Get())
                .Where(x => x.StudentId == studentId);

            if (!classAttendances.Any())
                throw new NoClassAttendanceFoundException();

            return classAttendances.Select(MapClassAttendanceToDTO);
        }

        public async Task<ClassAttendanceReturnDTO> GetAttendanceByClassAndStudent(int classId, int studentId)
        {
            await ValidateClassAndStudentExistence(classId, studentId);

            var classAttendance = (await _classAttendanceRepository.Get())
                .FirstOrDefault(x => x.ClassId == classId && x.StudentId == studentId);

            if (classAttendance == null)
                throw new NoSuchClassAttendanceException();

            return MapClassAttendanceToDTO(classAttendance);
        }

        public async Task<ClassAttendanceReturnDTO> UpdateClassAttendance(ClassAttendanceDTO classAttendanceDto)
        {
            await ValidateClassAndStudentExistence(classAttendanceDto.ClassId, classAttendanceDto.StudentId);

            if (!Enum.TryParse(classAttendanceDto.Status, out AttendanceStatus status))
                throw new InvalidAttendanceStatusException();

            var existingAttendance = (await _classAttendanceRepository.Get())
                .FirstOrDefault(x => x.ClassId == classAttendanceDto.ClassId && x.StudentId == classAttendanceDto.StudentId);

            if (existingAttendance == null)
                throw new NoSuchClassAttendanceException();

            existingAttendance.Status = status;

            var updatedAttendance = await _classAttendanceRepository.Update(existingAttendance);
            return MapClassAttendanceToDTO(updatedAttendance);
        }

        private async Task ValidateClassExistence(int classId)
        {
            if (await _classRepository.Get(classId) == null)
                throw new NoSuchClassException();
        }

        private async Task ValidateStudentExistence(int studentId)
        {
            if (await _studentRepository.Get(studentId) == null)
                throw new NoSuchStudentException();
        }

        private async Task ValidateClassAndStudentExistence(int classId, int studentId)
        {
            await ValidateClassExistence(classId);
            await ValidateStudentExistence(studentId);
        }

        private ClassAttendanceReturnDTO MapClassAttendanceToDTO(ClassAttendance attendance)
        {
            return new ClassAttendanceReturnDTO
            {
                ClassId = attendance.ClassId,
                StudentId = attendance.StudentId,
                Date = attendance.Date,
                Status = attendance.Status.ToString()
            };
        }
    }
}
