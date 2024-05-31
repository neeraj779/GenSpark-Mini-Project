using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Services
{
    public class CourseOfferingService : ICourseOfferingService
    {
        private readonly IRepository<int, CourseOffering> _courseOfferingRepository;
        private readonly IRepository<string, Course> _courseRepository;
        private readonly IRepository<int, Teacher> _teacherRepository;

        public CourseOfferingService(
            IRepository<int, CourseOffering> courseOfferingRepository,
            IRepository<string, Course> courseRepository,
            IRepository<int, Teacher> teacherRepository)
        {
            _courseOfferingRepository = courseOfferingRepository;
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
        }

        public async Task<CourseOfferingDTO> AssignTeacherForCourseOffering(int teacherId, string courseCode)
        {
            var course = await EnsureCourseExists(courseCode);
            var teacher = await EnsureTeacherExists(teacherId);

            var existingCourseOffering = await _courseOfferingRepository.Get();
            if (existingCourseOffering.Any(co => co.CourseCode == courseCode && co.TeacherId == teacherId))
                throw new CourseOfferingAlreadyExistsException();

            var newCourseOffering = new CourseOffering
            {
                CourseCode = course.CourseCode,
                TeacherId = teacher.TeacherId
            };

            var createdCourseOffering = await _courseOfferingRepository.Add(newCourseOffering);
            return MapToDTO(createdCourseOffering);
        }

        public async Task<IEnumerable<CourseOfferingDTO>> GetAllCourseOfferings()
        {
            var courseOfferings = await _courseOfferingRepository.Get();
            if (!courseOfferings.Any())
                throw new NoCourseOfferingException();

            return courseOfferings.Select(MapToDTO).ToList();
        }

        public async Task<IEnumerable<CourseOfferingDTO>> GetCourseOfferingByCourseCode(string courseCode)
        {
            var course = await EnsureCourseExists(courseCode);

            var filteredCourseOfferings = await FilterCourseOfferings(co => co.CourseCode == courseCode);
            if (!filteredCourseOfferings.Any())
                throw new NoCourseOfferingException();

            return filteredCourseOfferings.Select(MapToDTO).ToList();
        }

        public async Task<IEnumerable<CourseOfferingDTO>> GetCourseOfferingByTeacherId(int teacherId)
        {
            var teacher = await EnsureTeacherExists(teacherId);

            var filteredCourseOfferings = await FilterCourseOfferings(co => co.TeacherId == teacherId);
            if (!filteredCourseOfferings.Any())
                throw new NoCourseOfferingException();

            return filteredCourseOfferings.Select(MapToDTO).ToList();
        }

        private async Task<IEnumerable<CourseOffering>> FilterCourseOfferings(Func<CourseOffering, bool> filter)
        {
            var courseOfferings = await _courseOfferingRepository.Get();
            var filteredCourseOfferings = courseOfferings.Where(filter).ToList();
            return filteredCourseOfferings;
        }

        public async Task<CourseOfferingDTO> UnassignTeacherFromCourseOffering(int teacherId, string courseCode)
        {
            var course = await EnsureCourseExists(courseCode);
            var teacher = await EnsureTeacherExists(teacherId);

            var courseOfferings = await _courseOfferingRepository.Get();
            var courseOfferingToUnassign = courseOfferings.FirstOrDefault(co => co.CourseCode == courseCode && co.TeacherId == teacherId);
            if (courseOfferingToUnassign == null)
                throw new NoSuchCourseOfferingException();

            await _courseOfferingRepository.Delete(courseOfferingToUnassign.CourseOfferingId);
            return MapToDTO(courseOfferingToUnassign);
        }

        public async Task<CourseOfferingDTO> UpdateTeacherForCourseOffering(int teacherId, int courseOfferingId)
        {
            var teacher = await EnsureTeacherExists(teacherId);
            var courseOffering = await EnsureCourseOfferingExists(courseOfferingId);

            courseOffering.TeacherId = teacher.TeacherId;
            var updatedCourseOffering = await _courseOfferingRepository.Update(courseOffering);

            return MapToDTO(updatedCourseOffering);
        }

        private async Task<Course> EnsureCourseExists(string courseCode)
        {
            var course = await _courseRepository.Get(courseCode);
            if (course == null)
                throw new NoSuchCourseException();
            return course;
        }

        private async Task<Teacher> EnsureTeacherExists(int teacherId)
        {
            var teacher = await _teacherRepository.Get(teacherId);
            if (teacher == null)
                throw new NoSuchTeacherException();
            return teacher;
        }

        private async Task<CourseOffering> EnsureCourseOfferingExists(int courseOfferingId)
        {
            var courseOffering = await _courseOfferingRepository.Get(courseOfferingId);
            if (courseOffering == null)
                throw new NoSuchCourseOfferingException();
            return courseOffering;
        }

        private CourseOfferingDTO MapToDTO(CourseOffering courseOffering)
        {
            return new CourseOfferingDTO
            {
                CourseOfferingId = courseOffering.CourseOfferingId,
                CourseCode = courseOffering.CourseCode,
                TeacherId = courseOffering.TeacherId
            };
        }
    }
}
