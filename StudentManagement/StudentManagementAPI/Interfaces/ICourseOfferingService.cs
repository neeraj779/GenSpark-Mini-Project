using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;
using System.Threading.Tasks;

namespace StudentManagementAPI.Interfaces
{
    public interface ICourseOfferingService
    {
        public Task<CourseOfferingDTO> AssignTeacherForCourseOffering(int teacherid, string CourseCode);
        public Task<CourseOfferingDTO> UpdateTeacherForCourseOffering(int teacherid, string CourseCode);
        public Task<CourseOfferingDTO> UnassignTeacherFromCourseOffering(int teacherid, string CourseCode);
        public Task<IEnumerable<CourseOfferingDTO>> GetcourseOfferingByTeacherId(int teacherId);
        public Task<IEnumerable<CourseOfferingDTO>> GetcourseOfferingByCourseCode(string CourseCode);
        public Task<IEnumerable<CourseOfferingDTO>> GetAllCourseOfferings();

    }
}
