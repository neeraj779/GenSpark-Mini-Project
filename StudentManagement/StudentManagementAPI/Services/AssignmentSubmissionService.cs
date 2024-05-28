using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;
using System.Security.Claims;

namespace StudentManagementAPI.Services
{
    public class AssignmentSubmissionService : IAssignmentSubmissionService
    {
        public readonly IRepository<int, Submission> _sumissionRepository;


        public AssignmentSubmissionService(IRepository<int, Submission> sumissionRepository)
        {
            _sumissionRepository = sumissionRepository;
        }

        public async Task<AssignmentSubmisssionReturnDTO> SubmitAssignment(int studentId, AssignmentSubmisssionDTO assignmentSubmission)
        {
            Submission submission = new Submission();
            submission.StudentId = studentId;
            submission.SubmissionDate = DateTime.Now;
            submission.IsCompleted = assignmentSubmission.IsCompleted;

            var createdSubmission = await _sumissionRepository.Add(submission);

            return MapSubmissionToSubmissionDTO(createdSubmission);
        }

        public AssignmentSubmisssionReturnDTO MapSubmissionToSubmissionDTO(Submission submission)
        {
            AssignmentSubmisssionReturnDTO assignmentSubmisssionReturnDTO = new AssignmentSubmisssionReturnDTO();
            assignmentSubmisssionReturnDTO.SubmissionDate = submission.SubmissionDate;
            assignmentSubmisssionReturnDTO.stautus = submission.IsCompleted ? "Completed" : "Not Completed";

            return assignmentSubmisssionReturnDTO;
        }
    }
}
