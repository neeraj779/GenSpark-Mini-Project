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
    public class ClassAttendanceController : ControllerBase
    {
        private readonly IClassAttendanceService _classAttendanceService;

        public ClassAttendanceController(IClassAttendanceService classAttendanceService)
        {
            _classAttendanceService = classAttendanceService;
        }

        /// <summary>
        /// Marks the attendance of a student in a class.
        /// </summary>
        /// <param name="classAttendancedto"> ClassAttendanceDTO object containing the details of the class attendance to be marked.</param>
        /// <returns></returns>
        [HttpPost("MarkStudentAttendance")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(ClassAttendanceReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ClassAttendanceReturnDTO>> MarkStudentAttendance(ClassAttendanceDTO classAttendancedto)
        {
            try
            {
                var classAttendance = await _classAttendanceService.MarkStudentAttendance(classAttendancedto);
                return Ok(classAttendance);
            }
            catch (NoSuchClassException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (NoSuchStudentException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (ClassAttendanceAlreadyExistsException ex)
            {
                return Conflict(new ErrorModel { ErrorCode = StatusCodes.Status409Conflict, ErrorMessage = ex.Message });
            }
            catch (InvalidAttendanceStatusException ex)
            {
                return BadRequest(new ErrorModel { ErrorCode = StatusCodes.Status400BadRequest, ErrorMessage = ex.Message });
            }
            catch(NotEnrolledInCourseException ex)
            {
                return BadRequest(new ErrorModel { ErrorCode = StatusCodes.Status400BadRequest, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Gets the attendance of a class.
        /// </summary>
        /// <param name="classId"> The Id of the class for which the attendance is to be fetched.</param>
        /// <returns></returns>
        [HttpGet("GetAttendanceByClass")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(ClassAttendanceReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassAttendanceReturnDTO>> GetAttendanceByClass(int classId)
        {
            try
            {
                var classAttendanceList = await _classAttendanceService.GetAttendanceByClass(classId);
                return Ok(classAttendanceList);
            }
            catch (NoSuchClassException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (NoClassAttendanceFoundException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }


        }

        /// <summary>
        /// Gets the attendance of a student.
        /// </summary>
        /// <param name="studentId"> The Id of the student for which the attendance is to be fetched.</param>
        /// <returns></returns>
        [HttpGet("GetAttendanceByStudent")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(ClassAttendanceReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassAttendanceReturnDTO>> GetAttendanceByStudent(int studentId)
        {
            try
            {
                var classAttendanceList = await _classAttendanceService.GetAttendanceByStudent(studentId);
                return Ok(classAttendanceList);
            }
            catch (NoSuchStudentException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (NoClassAttendanceFoundException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }


        /// <summary>
        /// Gets the attendance of a student in a class.
        /// </summary>
        /// <param name="classId"> The Id of the class for which the attendance is to be fetched.</param>
        /// <param name="studentId"> The Id of the student for which the attendance is to be fetched.</param>
        /// <returns></returns>
        [HttpGet("GetAttendanceByClassAndStudent")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(ClassAttendanceReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassAttendanceReturnDTO>> GetAttendanceByClassAndStudent(int classId, int studentId)
        {
            try
            {
                var classAttendance = await _classAttendanceService.GetAttendanceByClassAndStudent(classId, studentId);
                return Ok(classAttendance);
            }
            catch (NoSuchClassException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (NoSuchStudentException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (NoSuchClassAttendanceException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Updates the attendance of a student in a class.
        /// </summary>
        /// <param name="classAttendancedto"> ClassAttendanceDTO object containing the details of the class attendance to be updated.</param>
        /// <returns></returns>
        [HttpPut("UpdateAttendance")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(ClassAttendanceReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassAttendanceReturnDTO>> UpdateAttendance(ClassAttendanceDTO classAttendancedto)
        {
            try
            {
                var classAttendance = await _classAttendanceService.UpdateClassAttendance(classAttendancedto);
                return Ok(classAttendance);
            }
            catch (NoSuchClassException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (NoSuchStudentException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (NoSuchClassAttendanceException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (InvalidAttendanceStatusException ex)
            {
                return BadRequest(new ErrorModel { ErrorCode = StatusCodes.Status400BadRequest, ErrorMessage = ex.Message });
            }
        }
    }
}
