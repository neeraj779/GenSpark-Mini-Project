using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Interfaces
{
    public interface ICourseOfferingService
    {
        public Task<CourseOfferingDTO> AssignTeacherForCourseOffering(int teacherid, string CourseCode);
        public Task<CourseOfferingDTO> UpdateTeacherForCourseOffering(int teacherid, int courseOfferingId);
        public Task<CourseOfferingDTO> UnassignTeacherFromCourseOffering(int teacherid, string CourseCode);
        public Task<IEnumerable<CourseOfferingDTO>> GetcourseOfferingByTeacherId(int teacherId);
        public Task<IEnumerable<CourseOfferingDTO>> GetcourseOfferingByCourseCode(string CourseCode);
        public Task<IEnumerable<CourseOfferingDTO>> GetAllCourseOfferings();

    }
}
