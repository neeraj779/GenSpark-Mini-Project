﻿using Microsoft.AspNetCore.Authorization;
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
        /// <returns>
        /// An action result containing the newly created assignment if successful.
        /// </returns>
        /// <response code="200">Returns the newly created assignment.</response>
        [HttpPost("CreateAssignment")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<ActionResult<AssignmentDTO>> CreateAssignment(CreateAssignmentDTO assignment)
        {
            try
            {
                var newAssignment = await _assignmentService.CreateAssignment(assignment);
                return Ok(newAssignment);
            }

            catch(NoSuchCourseException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Deletes an assignment based on the provided assignment ID.
        /// </summary>
        /// <param name="assignmentId">The ID of the assignment to be deleted.</param>
        /// <returns>
        /// An action result indicating the success of the deletion operation.
        /// </returns>
        /// <response code="200">If the assignment is successfully deleted.</response>
        [HttpDelete("DeleteAssignment")]
        [Authorize(Roles = "Admin, Teacher")]
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
        /// <returns>
        /// An action result containing the updated assignment if successful.
        /// </returns>
        /// <response code="200">Returns the updated assignment.</response>
        [HttpPut("UpdateAssignmentDueDate")]
        [Authorize(Roles = "Admin, Teacher")]
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
        /// <returns>
        /// An action result containing the assignment's details if found.
        /// </returns>
        /// <response code="200">Returns the assignment's details.</response>
        /// <response code="404">If the assignment with the given ID is not found.</response>
        [HttpGet("GetAssignmentById")]
        [Authorize(Roles = "Admin, Teacher, Student")]
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
        /// <returns>
        /// An action result containing the list of all assignments.
        /// </returns>
        /// <response code="200">Returns the list of all assignments.</response>
        [HttpGet("GetAllAssignments")]
        [Authorize(Roles = "Admin, Teacher, Student")]
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
