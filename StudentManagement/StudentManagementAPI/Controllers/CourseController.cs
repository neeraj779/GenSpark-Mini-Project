using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models;
using StudentManagementAPI.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        /// <summary>
        /// Creates a new course with the provided course details.
        /// </summary>
        [HttpPost("CreateCourse")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(CourseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CourseDTO>> CreateCourse(CourseDTO course)
        {
            try
            {
                var newCourse = await _courseService.CreateCourse(course);
                return Ok(newCourse);
            }

            catch (CourseAlreadyExistsException ex)
            {
                return Conflict(new ErrorModel { ErrorCode = StatusCodes.Status409Conflict, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a course based on the provided course code.
        /// </summary>
        /// <param name="courseCode">The code of the course to be deleted.</param>
        [HttpDelete("DeleteCourse")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(CourseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CourseDTO>> DeleteCourse(string courseCode)
        {
            try
            {
                var deletedCourse = await _courseService.DeleteCourse(courseCode);
                return Ok(deletedCourse);
            }

            catch (NoSuchCourseException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Updates the credit hours of an existing course based on the provided course code.
        /// </summary>
        /// <param name="courseCode">The code of the course to be updated.</param>
        /// <param name="creditHours">The new credit hours for the course.</param>
        [HttpPut("UpdateCourseCreditHours")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(CourseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CourseDTO>> UpdateCourseCreditHours(string courseCode, int creditHours)
        {
            try
            {
                var updatedCourse = await _courseService.UpdateCourseCreditHours(courseCode, creditHours);
                return Ok(updatedCourse);
            }

            catch (NoSuchCourseException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a course's details based on the provided course code.
        /// </summary>
        /// <param name="courseCode">The code of the course to be retrieved.</param>
        [HttpGet("GetCourseByCode")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        [ProducesResponseType(typeof(CourseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CourseDTO>> GetCourseByCode(string courseCode)
        {
            try
            {
                var course = await _courseService.GetCourseByCode(courseCode);
                return Ok(course);
            }

            catch (NoSuchCourseException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a list of all courses.
        /// </summary>
        [HttpGet("GetAllCourses")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        [ProducesResponseType(typeof(CourseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetCourses()
        {
            try
            {
                var courses = await _courseService.GetCourses();
                return Ok(courses);
            }

            catch (NoCourseFoundException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }
    }
}
