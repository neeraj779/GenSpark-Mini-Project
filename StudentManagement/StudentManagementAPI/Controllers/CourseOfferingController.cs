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

        /// <summary>
        /// Assigns a teacher to a course offering.
        /// </summary>
        /// <param name="teacherid"> The Id of the teacher to be assigned.</param>
        /// <param name="CourseCode"> The code of the course offering to which the teacher is to be assigned.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates the teacher assigned to a course offering.
        /// </summary>
        /// <param name="teacherid"> The Id of the teacher to be assigned.</param>
        /// <param name="CourseCode"> The code of the course offering to which the teacher is to be assigned.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Unassigns a teacher from a course offering.
        /// </summary>
        /// <param name="teacherid"> The Id of the teacher to be unassigned.</param>
        /// <param name="CourseCode"> The code of the course offering from which the teacher is to be unassigned.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the course offering details by teacher Id.
        /// </summary>
        /// <param name="teacherid"> The Id of the teacher for which the course offering details are to be fetched.</param> 
        /// <returns></returns>
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

        /// <summary>
        /// Gets the course offering details by course code.
        /// </summary>
        /// <param name="CourseCode"> The code of the course for which the course offering details are to be fetched.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets all the course offerings.
        /// </summary>
        /// <returns></returns>
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
