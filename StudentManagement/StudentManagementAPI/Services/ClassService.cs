using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Services
{
    public class ClassService : IClassService
    {
        private readonly IRepository<int, Class> _classRepository;
        private readonly IRepository<int, Teacher> _teacherRepository;
        private readonly IRepository<string, Course> _courseRepository;
        private readonly IRepository<int, CourseOffering> _courseOfferingRepository;

        public ClassService(
            IRepository<int, Class> classRepository,
            IRepository<int, Teacher> teacherRepository,
            IRepository<string, Course> courseRepository,
            IRepository<int, CourseOffering> courseOfferingRepository)
        {
            _classRepository = classRepository;
            _teacherRepository = teacherRepository;
            _courseRepository = courseRepository;
            _courseOfferingRepository = courseOfferingRepository;
        }

        public async Task<ClassReturnDTO> AddClass(ClassRegisterDTO classdto)
        {
            var IsClassExists = await _classRepository.Get();
            var classExists = IsClassExists.FirstOrDefault(classObj => classObj.CourseOfferingId == classdto.CourseOfferingId);

            if (classExists != null)
                throw new ClassAlreadyExistsException();

            var courseOffering = await _courseOfferingRepository.Get(classdto.CourseOfferingId);

            if (courseOffering == null)
                throw new NoSuchCourseOfferingException();

            var newClass = new Class
            {
                CourseOfferingId = classdto.CourseOfferingId,
                ClassDateAndTime = classdto.ClassDateAndTime
            };

            await _classRepository.Add(newClass);
            return await MapClassToClassReturnDTO(newClass);
        }

        public async Task<ClassReturnDTO> DeleteClass(int classId)
        {
            var classExists = await _classRepository.Get(classId);
            if (classExists == null)
                throw new NoSuchClassException();

            var deletedClass = await _classRepository.Delete(classId);
            return await MapClassToClassReturnDTO(deletedClass);
        }

        public async Task<ClassReturnDTO> GetClass(int classId)
        {
            var classExists = await _classRepository.Get(classId);
            if (classExists == null)
                throw new NoSuchClassException();

            var classObj = await _classRepository.Get(classId);
            return await MapClassToClassReturnDTO(classObj);
        }

        public async Task<IEnumerable<ClassReturnDTO>> GetClasses()
        {
            var classes = await _classRepository.Get();

            if (classes.Count() == 0)
                throw new NoClassFoundException();

            var classReturnDTOs = new List<ClassReturnDTO>();

            foreach (var classObj in classes)
            {
                classReturnDTOs.Add(await MapClassToClassReturnDTO(classObj));
            }

            return classReturnDTOs;
        }

        public async Task<ClassReturnDTO> UpdateClassTime(UpdateClassDTO updateclassdto)
        {
            var classExists = await _classRepository.Get(updateclassdto.ClassId);
            if (classExists == null)
                throw new NoSuchClassException();

            var classObj = await _classRepository.Get(updateclassdto.ClassId);
            classObj.ClassDateAndTime = updateclassdto.ClassDateAndTime;
            await _classRepository.Update(classObj);
            return await MapClassToClassReturnDTO(classObj);
        }

        private async Task<ClassReturnDTO> MapClassToClassReturnDTO(Class classEntity)
        {
            var courseOffering = await _courseOfferingRepository.Get(classEntity.CourseOfferingId);
            var course = await _courseRepository.Get(courseOffering.CourseCode);
            var teacher = await _teacherRepository.Get(courseOffering.TeacherId);

            return new ClassReturnDTO
            {
                ClassId = classEntity.ClassId,
                CourseOfferingId = classEntity.CourseOfferingId,
                ClassDateAndTime = classEntity.ClassDateAndTime,
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                TeacherId = teacher.TeacherId,
                TeacherName = teacher.FullName
            };
        }
    }
}
