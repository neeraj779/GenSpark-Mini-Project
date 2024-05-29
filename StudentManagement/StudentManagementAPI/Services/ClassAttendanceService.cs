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

        public ClassAttendanceService(IRepository<int, ClassAttendance> classAttendanceRepository, IRepository<int, Class> classRepository, IRepository<int, Student> studentRepository)
        {
            _classAttendanceRepository = classAttendanceRepository;
            _classRepository = classRepository;
            _studentRepository = studentRepository;
        }

        public async Task<ClassAttendanceReturnDTO> MarkStudentAttendance(ClassAttendanceDTO classAttendancedto)
        {
            var isClassExist = await _classRepository.Get(classAttendancedto.ClassId);
            if (isClassExist == null)
                throw new NoSuchClassException();

            var isStudentExist = await _studentRepository.Get(classAttendancedto.StudentId);
            if (isStudentExist == null)
                throw new NoSuchStudentException();

            var isAttendanceExist = await _classAttendanceRepository.Get();
            var attendanceExist = isAttendanceExist.FirstOrDefault(x => x.ClassId == classAttendancedto.ClassId && x.StudentId == classAttendancedto.StudentId);

            if (attendanceExist != null)
                throw new ClassAttendanceAlreadyExistsException();

            var classAttendance = new ClassAttendance
            {
                ClassId = classAttendancedto.ClassId,
                StudentId = classAttendancedto.StudentId,
                Date = DateTime.Now,
                Status = classAttendancedto.Status
            };

            var newAttendance = await _classAttendanceRepository.Add(classAttendance);
            return MapClassAttendanceToClassAttendanceReturnDTO(newAttendance);
        }


        public async Task<IEnumerable<ClassAttendanceReturnDTO>> GetAttendanceByClass(int classId)
        {
            var isClassExist = await _classRepository.Get(classId);
            if (isClassExist == null)
                throw new NoSuchClassException();

            var classAttendance = await _classAttendanceRepository.Get();
            var classAttendanceList = classAttendance.Where(x => x.ClassId == classId);

            if (classAttendanceList.Count() == 0)
                throw new NoClassAttendanceFoundException();

            var classAttendanceReturnList = new List<ClassAttendanceReturnDTO>();
            foreach (var attendance in classAttendanceList)
                classAttendanceReturnList.Add(MapClassAttendanceToClassAttendanceReturnDTO(attendance));

            return classAttendanceReturnList;
        }

        public async Task<IEnumerable<ClassAttendanceReturnDTO>> GetAttendanceByStudent(int studentId)
        {
            var isStudentExist = await _studentRepository.Get(studentId);
            if (isStudentExist == null)
                throw new NoSuchStudentException();

            var classAttendance = await _classAttendanceRepository.Get();
            var classAttendanceList = classAttendance.Where(x => x.StudentId == studentId);

            if (classAttendanceList.Count() == 0)
                throw new NoClassAttendanceFoundException();

            var classAttendanceReturnList = new List<ClassAttendanceReturnDTO>();
            foreach (var attendance in classAttendanceList)
                classAttendanceReturnList.Add(MapClassAttendanceToClassAttendanceReturnDTO(attendance));

            return classAttendanceReturnList;
        }

        public async Task<ClassAttendanceReturnDTO> GetAttendanceByClassAndStudent(int classId, int studentId)
        {
            var isClassExist = await _classRepository.Get(classId);
            if (isClassExist == null)
                throw new NoSuchClassException();

            var isStudentExist = await _studentRepository.Get(studentId);
            if (isStudentExist == null)
                throw new NoSuchStudentException();

            var classAttendance = await _classAttendanceRepository.Get();
            var classAttendanceList = classAttendance.FirstOrDefault(x => x.ClassId == classId && x.StudentId == studentId);

            if (classAttendanceList == null)
                throw new NoSuchClassAttendanceException();

            return MapClassAttendanceToClassAttendanceReturnDTO(classAttendanceList);
        }

        public async Task<ClassAttendanceReturnDTO> UpdateClassAttendance(ClassAttendanceDTO classAttendancedto)
        {
            var isClassExist = await _classRepository.Get(classAttendancedto.ClassId);
            if (isClassExist == null)
                throw new NoSuchClassException();

            var isStudentExist = await _studentRepository.Get(classAttendancedto.StudentId);
            if (isStudentExist == null)
                throw new NoSuchStudentException();

            var isAttendanceExist = await _classAttendanceRepository.Get();
            var attendanceExist = isAttendanceExist.FirstOrDefault(x => x.ClassId == classAttendancedto.ClassId && x.StudentId == classAttendancedto.StudentId);
            if (attendanceExist == null)
                throw new NoSuchClassAttendanceException();

            attendanceExist.Status = classAttendancedto.Status;

            var updatedAttendance = await _classAttendanceRepository.Update(attendanceExist);
            return MapClassAttendanceToClassAttendanceReturnDTO(updatedAttendance);
        }

        private ClassAttendanceReturnDTO MapClassAttendanceToClassAttendanceReturnDTO(ClassAttendance newAttendance)
        {
            return new ClassAttendanceReturnDTO
            {
                ClassId = newAttendance.ClassId,
                StudentId = newAttendance.StudentId,
                Date = newAttendance.Date,
                Status = newAttendance.Status
            };
        }
    }
}
