using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Interfaces
{
    public interface ICourseOfferingService
    {
        public Task<CourseOfferingDTO> AssignTeacherForCourseOffering(int teacherid, string CourseCode);
        public Task<CourseOfferingDTO> UpdateTeacherForCourseOffering(int teacherid, int courseOfferingId);
        public Task<CourseOfferingDTO> UnassignTeacherFromCourseOffering(int teacherid, string CourseCode);
        public Task<IEnumerable<CourseOfferingDTO>> GetCourseOfferingByTeacherId(int teacherId);
        public Task<IEnumerable<CourseOfferingDTO>> GetCourseOfferingByCourseCode(string CourseCode);
        public Task<IEnumerable<CourseOfferingDTO>> GetAllCourseOfferings();

    }
}
