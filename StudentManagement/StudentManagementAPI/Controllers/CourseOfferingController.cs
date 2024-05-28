using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseOfferingController : ControllerBase
    {
        public ICourseOfferingService _courseOfferingService;

        public CourseOfferingController(ICourseOfferingService courseOfferingService)
        {
            _courseOfferingService = courseOfferingService;
        }

        [HttpPost("AssignTeacherForCourseOffering")]
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
            catch(CourseOfferingAlreadyExistsException ex)
            {
                return Conflict(new ErrorModel { ErrorCode = StatusCodes.Status409Conflict, ErrorMessage = ex.Message });
            }
        }

        [HttpPost("UpdateTeacherForCourseOffering")]
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
            catch(NoSuchCourseOfferingException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        [HttpGet("GetCourseOfferingByTeacherId")]
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
        }

        [HttpGet("GetCourseOfferingByCourseCode")]
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
        }

        [HttpGet("GetAllCourseOfferings")]
        public async Task<ActionResult<CourseOfferingDTO>> GetAllCourseOfferings()
        {
            try
            {
                var courseOffering = await _courseOfferingService.GetAllCourseOfferings();
                return Ok(courseOffering);
            }
            catch (NoSuchCourseOfferingException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }
    }
}
