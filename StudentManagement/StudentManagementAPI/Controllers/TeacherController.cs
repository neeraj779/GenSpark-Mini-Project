using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        /// <summary>
        /// Registers a new teacher with the provided registration details.
        /// </summary>
        /// <param name="teacher">The registration details of the teacher.</param>
        /// <returns>
        /// An action result containing the newly created teacher if successful, 
        /// or an appropriate error message if the registration fails.
        /// </returns>
        /// <response code="200">Returns the newly created teacher.</response>
        /// <response code="400">If the registration fails due to invalid input.</response>
        [HttpPost("RegisterTeacher")]
        public async Task<ActionResult<TeacherReturnDTO>> AddTeacher(TeacherRegisterDTO teacher)
        {
            try
            {
                var newTeacher = await _teacherService.RegisterTeacher(teacher);
                return Ok(newTeacher);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a teacher's details based on the provided teacher ID.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher to be retrieved.</param>
        /// <returns>
        /// An action result containing the teacher's details if found.
        /// </returns>
        /// <response code="200">Returns the teacher's details.</response>
        /// <response code="404">If the teacher with the given ID is not found.</response>
        [HttpGet("GetTeacherById")]
        public async Task<ActionResult<TeacherReturnDTO>> GetTeacherById(int teacherId)
        {
            try
            {
                var teacher = await _teacherService.GetTeacherById(teacherId);
                return Ok(teacher);
            }

            catch (NoSuchTeacherException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a list of all teachers.
        /// </summary>
        /// <returns>
        /// An action result containing the list of all teachers.
        /// </returns>
        /// <response code="200">Returns the list of all teachers.</response>
        [HttpGet("GetAllTeachers")]
        public async Task<ActionResult<TeacherReturnDTO>> GetTeachers()
        {
            try
            {
                var teachers = await _teacherService.GetTeachers();
                return Ok(teachers);
            }

            catch (NoTeacherFoundException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        [HttpPut("UpdateTeacherEmail")]
        public async Task<ActionResult<TeacherReturnDTO>> UpdateTeacherEmail(int teacherId, string email)
        {
            try
            {
                var updatedTeacher = await _teacherService.UpdateTeacherEmail(teacherId, email);
                return Ok(updatedTeacher);
            }
            catch (NoSuchTeacherException)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = "Teacher not found" });
            }
        }

        [HttpPut("UpdateTeacherPhone")]
        public async Task<ActionResult<TeacherReturnDTO>> UpdateTeacherPhone(int teacherId, string phone)
        {
            try
            {
                var updatedTeacher = await _teacherService.UpdateTeacherPhone(teacherId, phone);
                return Ok(updatedTeacher);
            }
            catch (NoSuchTeacherException)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = "Teacher not found" });
            }
        }

        /// <summary>
        /// Deletes a teacher based on the provided teacher ID.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher to be deleted.</param>
        /// <returns>
        /// An action result indicating the success of the deletion operation.
        /// </returns>
        /// <response code="200">If the teacher is successfully deleted.</response>
        /// <response code="400">If the deletion fails due to invalid input.</response>
        [HttpDelete("DeleteTeacherRecord")]
        public async Task<ActionResult<TeacherReturnDTO>> DeleteTeacher(int teacherId)
        {
            try
            {
                var deletedTeacher = await _teacherService.DeleteTeacher(teacherId);
                return Ok(deletedTeacher);
            }
            catch (NoSuchTeacherException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }
    }
}
