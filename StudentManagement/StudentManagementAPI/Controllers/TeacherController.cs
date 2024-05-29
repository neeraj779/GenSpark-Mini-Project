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
        [HttpPost("RegisterTeacher")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(TeacherReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TeacherReturnDTO>> AddTeacher(TeacherRegisterDTO teacher)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newTeacher = await _teacherService.RegisterTeacher(teacher);
                return Ok(newTeacher);
            }
            catch (UnableToAddException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel { ErrorCode = StatusCodes.Status500InternalServerError, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a teacher's details based on the provided teacher ID.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher to be retrieved.</param>
        [HttpGet("GetTeacherById")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(TeacherReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
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
        [HttpGet("GetAllTeachers")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(TeacherReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Updates a teacher's email based on the provided teacher ID.
        /// </summary>
        /// <param name="updateEmaildto"> The ID of the teacher and the new email to be updated.</param>
        [HttpPut("UpdateTeacherEmail")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(TeacherReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TeacherReturnDTO>> UpdateTeacherEmail(UpdateEmailDTO updateEmaildto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedTeacher = await _teacherService.UpdateTeacherEmail(updateEmaildto);
                return Ok(updatedTeacher);
            }
            catch (NoSuchTeacherException)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = "Teacher not found" });
            }
        }

        /// <summary>
        /// Updates a teacher's phone number based on the provided teacher ID.
        /// </summary>
        /// <param name="updatePhonedto"> The ID of the teacher and the new phone number to be updated.</param>
        /// <returns></returns>
        [HttpPut("UpdateTeacherPhone")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(TeacherReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TeacherReturnDTO>> UpdateTeacherPhone(UpdatePhoneDTO updatePhonedto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedTeacher = await _teacherService.UpdateTeacherPhone(updatePhonedto);
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
        [HttpDelete("DeleteTeacherRecord")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(TeacherReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
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
