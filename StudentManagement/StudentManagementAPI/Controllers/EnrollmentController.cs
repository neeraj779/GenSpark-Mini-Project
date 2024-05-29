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
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost("EnrollStudentInCourse")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> EnrollStudent([FromBody] EnrollmentDTO enrollment)
        {
            try
            {
                var result = await _enrollmentService.EnrollStudent(enrollment.StudentId, enrollment.CourseCode);
                return Ok($"Student with ID {result.StudentId} enrolled in course with code {result.CourseCode}");
            }

            catch (NoSuchStudentException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }

            catch (NoSuchCourseException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }

            catch (StudentAlreadyEnrolledException ex)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ErrorModel { ErrorCode = StatusCodes.Status409Conflict, ErrorMessage = ex.Message });
            }
        }

        [HttpPost("UnenrollStudentFromCourse")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> UnenrollStudent([FromBody] EnrollmentDTO enrollment)
        {
            try
            {
                var result = await _enrollmentService.UnenrollStudent(enrollment.StudentId, enrollment.CourseCode);
                return Ok($"Student with Id {result.StudentId} unenrolled from course with code {result.CourseCode}");
            }

            catch (NoSuchStudentException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }

            catch (NoSuchCourseException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        [HttpGet("GetEnrollmentsByStudentId")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        public async Task<ActionResult<EnrollmentReturnDTO>> GetEnrollmentsByStudentId(int studentId)
        {
            try
            {
                var result = await _enrollmentService.GetEnrollmentsByStudentId(studentId);
                return Ok(result);
            }

            catch (NoSuchEnrollmentException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        [HttpGet("GetEnrollmentsByCourseId")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        public async Task<ActionResult<EnrollmentReturnDTO>>  GetEnrollmentsByCourseId(string courseCode)
        {
            try
            {
                var result = await _enrollmentService.GetEnrollmentsByCourseId(courseCode);
                return Ok(result);
            }

            catch (NoSuchEnrollmentException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        [HttpGet("GetAllEnrollments")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<ActionResult<EnrollmentReturnDTO>> GetAllEnrollments()
        {
            try
            {
                var result = await _enrollmentService.GetAllEnrollments();
                return Ok(result);
            }

            catch (NoEnrollmentFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }
    }
}
