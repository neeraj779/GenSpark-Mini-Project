using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Interfaces
{
    public interface IAssignmentSubmissionService
    {
        public Task<IEnumerable<AssignmentDTO>> GetAssignedAssignments(int userId);
        public Task<IEnumerable<AssignmentDTO>> GetAssignedAssignmentsByCourse(int userId, string courseCode);
        public Task<AssignmentDTO> GetAssignmentSubmissionStatus(int userId, int assignmentId);
        public Task<AssignmentSubmisssionReturnDTO> SubmitAssignment(int userId, AssignmentSubmisssionDTO assignmentSubmission);
    }
}
