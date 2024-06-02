using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Services
{
    public class CourseService : ICourseService
    {
        private readonly IRepository<string, Course> _courseRepository;
        private readonly ILogger<CourseService> _logger;

        public CourseService(IRepository<string, Course> courseRepository, ILogger<CourseService> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }

        public async Task<CourseDTO> CreateCourse(CourseDTO courseDto)
        {
            var existingCourse = await _courseRepository.Get(courseDto.CourseCode);

            if (existingCourse != null)
            {
                _logger.LogWarning("Course with code {CourseCode} already exists", courseDto.CourseCode);
                throw new CourseAlreadyExistsException();
            }

            var newCourse = new Course
            {
                CourseCode = courseDto.CourseCode,
                CourseName = courseDto.CourseName,
                CourseCredit = courseDto.CourseCredit
            };

            var createdCourse = await _courseRepository.Add(newCourse);

            return MapToDTO(createdCourse);
        }

        public async Task<CourseDTO> DeleteCourse(string courseCode)
        {
            await EnsureCourseExists(courseCode);

            var deletedCourse = await _courseRepository.Delete(courseCode);
            return MapToDTO(deletedCourse);
        }

        public async Task<CourseDTO> GetCourseByCode(string courseCode)
        {
            var course = await EnsureCourseExists(courseCode);
            return MapToDTO(course);
        }

        public async Task<IEnumerable<CourseDTO>> GetCourses()
        {
            var courses = await _courseRepository.Get();
            if (!courses.Any())
            {
                _logger.LogWarning("No courses found");
                throw new NoCourseFoundException();
            }

            return courses.Select(MapToDTO).ToList();
        }

        public async Task<CourseDTO> UpdateCourseCreditHours(string courseCode, int creditHours)
        {
            var course = await EnsureCourseExists(courseCode);

            course.CourseCredit = creditHours;
            var updatedCourse = await _courseRepository.Update(course);

            return MapToDTO(updatedCourse);
        }

        private async Task<Course> EnsureCourseExists(string courseCode)
        {
            var course = await _courseRepository.Get(courseCode);
            if (course == null)
            {
                _logger.LogWarning("Course not found with code: {CourseCode}", courseCode);
                throw new NoSuchCourseException();
            }
            return course;
        }

        private CourseDTO MapToDTO(Course course)
        {
            return new CourseDTO
            {
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                CourseCredit = course.CourseCredit
            };
        }
    }
}
