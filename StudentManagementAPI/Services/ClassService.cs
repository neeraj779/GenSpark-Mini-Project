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

        public async Task<ClassReturnDTO> AddClass(ClassRegisterDTO classDto)
        {
            await EnsureClassDoesNotExist(classDto.CourseOfferingId, classDto.ClassDateAndTime);

            var courseOffering = await _courseOfferingRepository.Get(classDto.CourseOfferingId);
            if (courseOffering == null)
                throw new NoSuchCourseOfferingException();

            var newClass = new Class
            {
                CourseOfferingId = classDto.CourseOfferingId,
                ClassDateAndTime = classDto.ClassDateAndTime
            };

            var addedClass = await _classRepository.Add(newClass);
            return await MapClassToClassReturnDTO(addedClass);
        }

        public async Task<ClassReturnDTO> DeleteClass(int classId)
        {
            var classEntity = await EnsureClassExists(classId);
            var deletedClass = await _classRepository.Delete(classEntity.ClassId);
            return await MapClassToClassReturnDTO(deletedClass);
        }

        public async Task<ClassReturnDTO> GetClass(int classId)
        {
            var classEntity = await EnsureClassExists(classId);
            return await MapClassToClassReturnDTO(classEntity);
        }

        public async Task<IEnumerable<ClassReturnDTO>> GetClasses()
        {
            var classes = await _classRepository.Get();
            if (!classes.Any())
                throw new NoClassFoundException();

            var classReturnDTOs = new List<ClassReturnDTO>();
            foreach (var classObj in classes)
            {
                classReturnDTOs.Add(await MapClassToClassReturnDTO(classObj));
            }

            return classReturnDTOs;
        }

        public async Task<ClassReturnDTO> UpdateClassTime(UpdateClassDTO updateClassDto)
        {
            var classEntity = await EnsureClassExists(updateClassDto.ClassId);

            classEntity.ClassDateAndTime = updateClassDto.ClassDateAndTime;
            var updatedClass = await _classRepository.Update(classEntity);
            return await MapClassToClassReturnDTO(updatedClass);
        }

        private async Task<Class> EnsureClassExists(int classId)
        {
            var classEntity = await _classRepository.Get(classId);
            if (classEntity == null)
                throw new NoSuchClassException();

            return classEntity;
        }

        private async Task EnsureClassDoesNotExist(int courseOfferingId, DateTime classDateAndTime)
        {
            var existingClasses = await _classRepository.Get();
            var classExists = existingClasses.Any(classObj =>
                classObj.CourseOfferingId == courseOfferingId && classObj.ClassDateAndTime == classDateAndTime);

            if (classExists)
                throw new ClassAlreadyExistsException();
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
