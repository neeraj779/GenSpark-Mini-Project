using log4net.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models;
using StudentManagementAPI.Models.DTOs;
using StudentManagementAPI.Services;

namespace StudentManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Registers a new student with the provided registration details.
        /// </summary>
        /// <param name="student"> StudentRegisterDTO object containing the details of the student to be registered.</param>
        [HttpPost("RegisterStudent")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(StudentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentReturnDTO>> AddStudent(StudentRegisterDTO student)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newStudent = await _studentService.RegisterStudent(student);
                return Ok(newStudent);
            }
            catch (InvalidStudentStatusException ex)
            {
                return BadRequest(new ErrorModel { ErrorCode = StatusCodes.Status400BadRequest, ErrorMessage = ex.Message });
            }

            catch (UnableToAddException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel { ErrorCode = StatusCodes.Status500InternalServerError, ErrorMessage = ex.Message });
            }
        }
        
        /// <summary>
        /// Updates the email of a student based on the provided student ID.
        /// </summary>
        /// <param name="updateEmaildto"> UpdateEmailDTO object containing the details of the student's email to be updated.</param>
        [HttpPut("UpdateStudentEmail")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(StudentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentReturnDTO>> UpdateStudentEmail(UpdateEmailDTO updateEmaildto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var updatedStudent = await _studentService.UpdateStudentEmail(updateEmaildto);
                return Ok(updatedStudent);
            }

            catch (NoSuchStudentException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Updates the phone number of a student based on the provided student ID.
        /// </summary>
        /// <param name="updatePhonedto"> UpdatePhoneDTO object containing the details of the student's phone number to be updated.</param>
        /// <returns></returns>
        [HttpPut("UpdateStudentPhone")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(StudentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentReturnDTO>> UpdateStudentPhone(UpdatePhoneDTO updatePhonedto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var updatedStudent = await _studentService.UpdateStudentPhone(updatePhonedto);
                return Ok(updatedStudent);
            }

            catch (NoSuchStudentException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Updates the status of a student based on the provided student ID.
        /// </summary>
        /// <param name="studentId"> The ID of the student to be updated.</param>
        /// <param name="status"> The new status for the student.</param>
        /// <returns></returns>
        [HttpPut("UpdateStudentStatus")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(StudentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentReturnDTO>> UpdateStudentStatus(int studentId, string status)
        {
            try
            {
                var updatedStudent = await _studentService.UpdateStudentStatus(studentId, status);
                return Ok(updatedStudent);
            }

            catch (NoSuchStudentException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }

            catch (InvalidStudentStatusException ex)
            {
                return BadRequest(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }


        /// <summary>
        /// Retrieves a student's details based on the provided student ID.
        /// </summary>
        /// <param name="studentId">The ID of the student to be retrieved.</param>
        [HttpGet("GetStudentById")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(StudentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentReturnDTO>> GetStudentById(int studentId)
        {
            try
            {
                var student = await _studentService.GetStudentById(studentId);
                return Ok(student);
            }

            catch (NoSuchStudentException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a list of all students.
        /// </summary>
        [HttpGet("GetAllStudents")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(StudentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentReturnDTO>> GetStudents()
        {
            try
            {
                var students = await _studentService.GetStudents();
                return Ok(students);
            }

            catch (NoStudentFoundException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a student based on the provided student ID.
        /// </summary>
        /// <param name="studentId">The ID of the student to be deleted.</param>
        [HttpDelete("DeleteStudent")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(StudentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentReturnDTO>> DeleteStudent(int studentId)
        {
            try
            {
                var deletedStudent = await _studentService.DeleteStudent(studentId);
                return Ok(deletedStudent);
            }

            catch (NoSuchStudentException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }
    }
}
