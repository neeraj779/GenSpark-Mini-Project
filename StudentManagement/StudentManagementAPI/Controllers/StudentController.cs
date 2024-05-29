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
        /// <param name="student">The registration details of the student.</param>
        /// <returns>
        /// An action result containing the newly created student if successful.
        /// </returns>
        /// <response code="200">Returns the newly created student.</response>
        [HttpPost("RegisterStudent")]
        [Authorize(Roles = "Admin, Teacher")]
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
        }

        [HttpPut("UpdateStudentEmail")]
        [Authorize(Roles = "Admin, Teacher")]
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

        [HttpPut("UpdateStudentPhone")]
        [Authorize(Roles = "Admin, Teacher")]
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

        [HttpPut("UpdateStudentStatus")]
        [Authorize(Roles = "Admin, Teacher")]
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
        }


        /// <summary>
        /// Retrieves a student's details based on the provided student ID.
        /// </summary>
        /// <param name="studentId">The ID of the student to be retrieved.</param>
        /// <returns>
        /// An action result containing the student's details if found.
        /// </returns>
        /// <response code="200">Returns the student's details.</response>
        /// <response code="404">If the student with the given ID is not found.</response>
        [HttpGet("GetStudentById")]
        [Authorize(Roles = "Admin, Teacher")]
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
        /// <returns>
        /// An action result containing the list of all students.
        /// </returns>
        /// <response code="200">Returns the list of all students.</response>
        [HttpGet("GetAllStudents")]
        [Authorize(Roles = "Admin, Teacher")]
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
        /// <returns>
        /// An action result indicating the success of the deletion operation.
        /// </returns>
        /// <response code="200">If the student is successfully deleted.</response>
        [HttpDelete("DeleteStudent")]
        [Authorize(Roles = "Admin, Teacher")]
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
