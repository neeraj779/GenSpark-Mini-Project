using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Services
{
    public class CourseService : ICourseService
    {
        private IRepository<string, Course> _courseRepository;

        public CourseService(IRepository<string, Course> courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<CourseDTO> CreateCourse(CourseDTO course)
        {
            var newCourse = new Course();

            newCourse.CourseCode = course.CourseCode;
            newCourse.CourseName = course.CourseName;
            newCourse.CourseCredit = course.CourseCredit;

            var createdCourse = await _courseRepository.Add(newCourse);

            return MapCourseToCourseDTO(createdCourse);
        }

        public async Task<CourseDTO> DeleteCourse(string courseCode)
        {
            var deletedCourse = await _courseRepository.Delete(courseCode);
            return MapCourseToCourseDTO(deletedCourse);
        }

        public async Task<CourseDTO> GetCourseByCode(string courseCode)
        {
            var course = await _courseRepository.Get(courseCode);
            return MapCourseToCourseDTO(course);
        }

        public async Task<IEnumerable<CourseDTO>> GetCourses()
        {
            var courses = await _courseRepository.Get();
            var courseDTOs = new List<CourseDTO>();
            foreach (var course in courses)
                courseDTOs.Add(MapCourseToCourseDTO(course));

            return courseDTOs;

        }

        public async Task<CourseDTO> UpdateCourseCreditHours(string courseCode, int creditHours)
        {
            var course = await _courseRepository.Get(courseCode);
            course.CourseCredit = creditHours;

            var updatedCourse = await _courseRepository.Update(course);
            return MapCourseToCourseDTO(updatedCourse);
        }

        public CourseDTO MapCourseToCourseDTO(Course course)
        {
            CourseDTO newcourse = new CourseDTO();
            newcourse.CourseCode = course.CourseCode;
            newcourse.CourseName = course.CourseName;
            newcourse.CourseCredit = course.CourseCredit;
            return newcourse;
        }
    }
}
