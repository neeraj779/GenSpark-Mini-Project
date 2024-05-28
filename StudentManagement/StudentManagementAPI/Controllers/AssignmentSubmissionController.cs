using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DTOs;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentSubmissionController : ControllerBase
    {
        private readonly IAssignmentSubmissionService _assignmentSubmission;

        public AssignmentSubmissionController(IAssignmentSubmissionService assignmentSubmission)
        {
            _assignmentSubmission = assignmentSubmission;
        }

        /// <summary>
        /// Submits an assignment with the provided submission details.
        /// </summary>
        /// <param name="assignmentSubmission">The details of the assignment submission.</param>
        /// <returns>
        /// An action result containing the details of the submitted assignment if successful.
        /// </returns>
        /// <response code="200">Returns the details of the submitted assignment.</response>
        [HttpPost("SubmitAssignment")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<AssignmentSubmisssionReturnDTO>> SubmitAssignment(AssignmentSubmisssionDTO assignmentSubmission)
        {
            string studentId = User.FindFirstValue(ClaimTypes.Name);

            int Id = Convert.ToInt32(studentId);

            var newAssignmentSubmission = await _assignmentSubmission.SubmitAssignment(Id, assignmentSubmission);
            return Ok(newAssignmentSubmission);
        }
    }
}
