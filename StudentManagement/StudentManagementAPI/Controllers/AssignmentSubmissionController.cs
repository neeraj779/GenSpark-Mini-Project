using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models;
using StudentManagementAPI.Models.DTOs;
using System.Security.Claims;

namespace StudentManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
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
        [HttpPost("SubmitAssignment")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(typeof(AssignmentSubmisssionReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AssignmentSubmisssionReturnDTO>> SubmitAssignment([FromForm] AssignmentSubmisssionDTO assignmentSubmission)
        {
            try
            {
                string studentId = User.FindFirstValue(ClaimTypes.Name);

                int Id = Convert.ToInt32(studentId);

                var newAssignmentSubmission = await _assignmentSubmission.SubmitAssignment(Id, assignmentSubmission);
                return Ok(newAssignmentSubmission);
            }
            catch (NoSuchAssignmentException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }

            catch(NotEnrolledInCourseException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch(DuplicateAssignmentSubmissionException ex)
            {
                return Conflict(new ErrorModel { ErrorCode = StatusCodes.Status409Conflict, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Gets all the assignments assigned to the student.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAssignedAssignments")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(typeof(IEnumerable<AssignmentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<AssignmentDTO>>> GetAssignedAssignments()
        {
            try
            {
                string studentId = User.FindFirstValue(ClaimTypes.Name);

                int Id = Convert.ToInt32(studentId);

                var assignments = await _assignmentSubmission.GetAssignedAssignments(Id);
                return Ok(assignments);
            }

            catch (NotEnrolledInAnyCourseException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }

            catch (NoAssignmentFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Gets all the assignments assigned to the student for a particular course.
        /// </summary>
        /// <param name="courseCode">The course code for which the assignments are to be fetched.</param>
        /// <returns></returns>
        [HttpGet("GetAssignedAssignmentsByCourse")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(typeof(IEnumerable<AssignmentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<AssignmentDTO>>> GetAssignedAssignmentsByCourse(string courseCode)
        {
            try
            {
                string studentId = User.FindFirstValue(ClaimTypes.Name);

                int Id = Convert.ToInt32(studentId);

                var assignments = await _assignmentSubmission.GetAssignedAssignmentsByCourse(Id, courseCode);
                return Ok(assignments);
            }

            catch (NotEnrolledInCourseException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }

            catch (NoAssignmentFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (NoSuchCourseException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

    }
}
