using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Interfaces
{
    public interface IClassService
    {
        public Task<ClassReturnDTO> AddClass(ClassRegisterDTO classdto);
        public Task<ClassReturnDTO> GetClass(int classId);
        public Task<ClassReturnDTO> DeleteClass(int classId);
        public Task<IEnumerable<ClassReturnDTO>> GetClasses();
        public Task<ClassReturnDTO> UpdateClassTime(UpdateClassDTO updateClassDTO);
    }
}
