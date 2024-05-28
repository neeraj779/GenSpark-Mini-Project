using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Interfaces
{
    public interface ICourseService
    {
        public Task<CourseDTO> CreateCourse(CourseDTO course);
        public Task<CourseDTO> DeleteCourse(string courseId);
        public Task<CourseDTO> GetCourseByCode(string courseId);

        public Task<CourseDTO> UpdateCourseCreditHours(string courseId, int creditHours);
        public Task<IEnumerable<CourseDTO>> GetCourses();
    }
}
