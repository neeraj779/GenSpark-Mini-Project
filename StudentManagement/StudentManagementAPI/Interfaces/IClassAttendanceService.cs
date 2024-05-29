using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Interfaces
{
    public interface IClassAttendanceService
    {
        public Task<ClassAttendanceReturnDTO> MarkStudentAttendance(ClassAttendanceDTO classAttendance);
        public Task<ClassAttendanceReturnDTO> GetAttendanceByClassAndStudent(int classId, int studentId);
        public Task<IEnumerable<ClassAttendanceReturnDTO>> GetAttendanceByClass(int classId);
        public Task<IEnumerable<ClassAttendanceReturnDTO>> GetAttendanceByStudent(int studentId);
        public Task<ClassAttendanceReturnDTO> UpdateClassAttendance(ClassAttendanceDTO classAttendance);
    }
}
