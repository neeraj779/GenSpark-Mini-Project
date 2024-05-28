using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Interfaces
{
    public interface IAssignmentSubmissionService
    {
        public Task<AssignmentSubmisssionReturnDTO> SubmitAssignment(int studentId, AssignmentSubmisssionDTO assignmentSubmission);
    }
}
