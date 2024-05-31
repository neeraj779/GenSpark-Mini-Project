using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        /// <summary>
        /// Creates a new assignment with the provided details.
        /// </summary>
        /// <param name="assignment">The details of the assignment to be created.</param>
        [HttpPost("CreateAssignment")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(AssignmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AssignmentDTO>> CreateAssignment(CreateAssignmentDTO assignment)
        {
            try
            {
                var newAssignment = await _assignmentService.CreateAssignment(assignment);
                return Ok(newAssignment);
            }

            catch (NoSuchCourseException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (AssignmentAlreadyExistsException ex)
            {
                return Conflict(new ErrorModel { ErrorCode = StatusCodes.Status409Conflict, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Deletes an assignment based on the provided assignment ID.
        /// </summary>
        /// <param name="assignmentId">The ID of the assignment to be deleted.</param>
        [HttpDelete("DeleteAssignment")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(AssignmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AssignmentDTO>> DeleteAssignment(int assignmentId)
        {
            try
            {
                var deletedAssignment = await _assignmentService.DeleteAssignment(assignmentId);
                return Ok(deletedAssignment);
            }

            catch (NoSuchAssignmentException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Updates the due date of an existing assignment based on the provided assignment ID.
        /// </summary>
        /// <param name="assignmentId">The ID of the assignment to be updated.</param>
        /// <param name="dueDate">The new due date for the assignment.</param>
        [HttpPut("UpdateAssignmentDueDate")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(AssignmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AssignmentDTO>> UpdateAssignmentDueDate(AssignmentUpdateDTO assignment)
        {
            try
            {
                var updatedAssignment = await _assignmentService.UpdateAssignmentDueDate(assignment);
                return Ok(updatedAssignment);
            }

            catch (NoSuchAssignmentException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves an assignment's details based on the provided assignment ID.
        /// </summary>
        /// <param name="assignmentId">The ID of the assignment to be retrieved.</param>
        [HttpGet("GetAssignmentById")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        [ProducesResponseType(typeof(AssignmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AssignmentDTO>> GetAssignmentById(int assignmentId)
        {
            try
            {
                var assignment = await _assignmentService.GetAssignmentById(assignmentId);
                return Ok(assignment);
            }

            catch (NoSuchAssignmentException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a list of all assignments.
        /// </summary>
        [HttpGet("GetAllAssignments")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        [ProducesResponseType(typeof(AssignmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<AssignmentDTO>>> GetAssignments()
        {
            try
            {
                var assignments = await _assignmentService.GetAssignments();
                return Ok(assignments);
            }

            catch (NoAssignmentFoundException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }
    }
}
