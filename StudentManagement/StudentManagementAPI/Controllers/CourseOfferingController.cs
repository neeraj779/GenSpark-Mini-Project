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
    public class CourseOfferingController : ControllerBase
    {
        public ICourseOfferingService _courseOfferingService;

        public CourseOfferingController(ICourseOfferingService courseOfferingService)
        {
            _courseOfferingService = courseOfferingService;
        }

        [HttpPost("AssignTeacherForCourseOffering")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CourseOfferingDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CourseOfferingDTO>> AssignTeacherForCourseOffering(int teacherid, string CourseCode)
        {
            try
            {
                var courseOffering = await _courseOfferingService.AssignTeacherForCourseOffering(teacherid, CourseCode);
                return Ok(courseOffering);
            }
            catch (NoSuchCourseException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (NoSuchTeacherException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (CourseOfferingAlreadyExistsException ex)
            {
                return Conflict(new ErrorModel { ErrorCode = StatusCodes.Status409Conflict, ErrorMessage = ex.Message });
            }
        }

        [HttpPost("UpdateTeacherForCourseOffering")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CourseOfferingDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CourseOfferingDTO>> UpdateTeacherForCourseOffering(int teacherid, string CourseCode)
        {
            try
            {
                var courseOffering = await _courseOfferingService.UpdateTeacherForCourseOffering(teacherid, CourseCode);
                return Ok(courseOffering);
            }
            catch (NoSuchCourseException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (NoSuchTeacherException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        [HttpPost("UnassignTeacherFromCourseOffering")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CourseOfferingDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CourseOfferingDTO>> UnassignTeacherFromCourseOffering(int teacherid, string CourseCode)
        {
            try
            {
                var courseOffering = await _courseOfferingService.UnassignTeacherFromCourseOffering(teacherid, CourseCode);
                return Ok(courseOffering);
            }
            catch (NoSuchCourseException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (NoSuchTeacherException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (NoSuchCourseOfferingException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        [HttpGet("GetCourseOfferingByTeacherId")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        [ProducesResponseType(typeof(CourseOfferingDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CourseOfferingDTO>> GetCourseOfferingByTeacherId(int teacherid)
        {
            try
            {
                var courseOffering = await _courseOfferingService.GetcourseOfferingByTeacherId(teacherid);
                return Ok(courseOffering);
            }
            catch (NoCourseOfferingException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (NoSuchTeacherException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        [HttpGet("GetCourseOfferingByCourseCode")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        [ProducesResponseType(typeof(CourseOfferingDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CourseOfferingDTO>> GetCourseOfferingByCourseCode(string CourseCode)
        {
            try
            {
                var courseOffering = await _courseOfferingService.GetcourseOfferingByCourseCode(CourseCode);
                return Ok(courseOffering);
            }
            catch (NoCourseOfferingException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
            catch (NoSuchCourseException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        [HttpGet("GetAllCourseOfferings")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        [ProducesResponseType(typeof(CourseOfferingDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CourseOfferingDTO>> GetAllCourseOfferings()
        {
            try
            {
                var courseOffering = await _courseOfferingService.GetAllCourseOfferings();
                return Ok(courseOffering);
            }
            catch (NoCourseOfferingException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }
    }
}
