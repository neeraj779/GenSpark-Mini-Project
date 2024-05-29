using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassAttendanceController : ControllerBase
    {
        private readonly IClassAttendanceService _classAttendanceService;

        public ClassAttendanceController(IClassAttendanceService classAttendanceService)
        {
            _classAttendanceService = classAttendanceService;
        }

        [HttpPost("MarkStudentAttendance")]
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
            catch(ClassAttendanceAlreadyExistsException ex)
            {
                return Conflict(new ErrorModel { ErrorCode = StatusCodes.Status409Conflict, ErrorMessage = ex.Message });
            }
        }

        [HttpGet("GetAttendanceByClass")]
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
        }

        [HttpGet("GetAttendanceByStudent")]
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
        }

        [HttpGet("GetAttendanceByClassAndStudent")]
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
        }

        [HttpPut("UpdateAttendance")]
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
            catch(NoSuchClassAttendanceException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }
    }
}
