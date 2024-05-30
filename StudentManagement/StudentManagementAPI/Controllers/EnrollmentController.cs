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

        /// <summary>
        /// Enrolls a student in a course.
        /// </summary>
        /// <param name="enrollment"> EnrollmentDTO object containing the details of the enrollment to be made.</param>
        /// <returns></returns>
        [HttpPost("EnrollStudentInCourse")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> EnrollStudent(EnrollmentDTO enrollment)
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

        /// <summary>
        /// Unenrolls a student from a course.
        /// </summary>
        /// <param name="enrollment"> EnrollmentDTO object containing the details of the enrollment to be removed.</param>
        /// <returns></returns>
        [HttpPost("UnenrollStudentFromCourse")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UnenrollStudent(EnrollmentDTO enrollment)
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

        /// <summary>
        /// Retrieves a student's enrollments based on the provided student ID.
        /// </summary>
        /// <param name="studentId"> The ID of the student for which the enrollments are to be fetched.</param>
        /// <returns></returns>
        [HttpGet("GetEnrollmentsByStudentId")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(EnrollmentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Retrieves a course's enrollments based on the provided course code.
        /// </summary>
        /// <param name="courseCode"> The code of the course for which the enrollments are to be fetched.</param>
        /// <returns></returns>
        [HttpGet("GetEnrollmentsByCourseId")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(EnrollmentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Retrieves all enrollments.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllEnrollments")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(EnrollmentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
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
