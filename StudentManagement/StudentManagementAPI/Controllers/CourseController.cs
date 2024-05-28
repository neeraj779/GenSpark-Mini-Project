using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models;
using StudentManagementAPI.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementAPI.Controllers
{
    [Route("api/[controller]")]
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
        /// <param name="course">The details of the course to be created.</param>
        /// <returns>
        /// An action result containing the newly created course if successful.
        /// </returns>
        /// <response code="200">Returns the newly created course.</response>
        [HttpPost("CreateCourse")]
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
        /// <returns>
        /// An action result indicating the success of the deletion operation.
        /// </returns>
        /// <response code="200">If the course is successfully deleted.</response>
        [HttpDelete("DeleteCourse")]
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
        /// <returns>
        /// An action result containing the updated course if successful.
        /// </returns>
        /// <response code="200">Returns the updated course.</response>
        [HttpPut("UpdateCourseCreditHours")]
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
        /// <returns>
        /// An action result containing the course's details if found.
        /// </returns>
        /// <response code="200">Returns the course's details.</response>
        /// <response code="404">If the course with the given code is not found.</response>
        [HttpGet("GetCourseByCode")]
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
        /// <returns>
        /// An action result containing the list of all courses.
        /// </returns>
        /// <response code="200">Returns the list of all courses.</response>
        [HttpGet("GetAllCourses")]
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
