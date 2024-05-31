using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Interfaces
{
    public interface IAssignmentService
    {
        public Task<AssignmentDTO> CreateAssignment(CreateAssignmentDTO assignment);
        public Task<AssignmentDTO> UpdateAssignmentDueDate(AssignmentUpdateDTO assignment);
        public Task<AssignmentDTO> DeleteAssignment(int assignmentId);
        public Task<AssignmentDTO> GetAssignmentById(int assignmentId);
        public Task<IEnumerable<AssignmentDTO>> GetAssignments();
    }
}
