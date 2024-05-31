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
    public class ClassController : ControllerBase
    {
        public readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        /// <summary>
        /// Adds a new class to the database.
        /// </summary>
        /// <param name="classdto"> ClassRegisterDTO object containing the details of the class to be added.</param>
        /// <returns></returns>
        [HttpPost("AddClass")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(ClassReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ClassReturnDTO>> AddClass(ClassRegisterDTO classdto)
        {
            try
            {
                var newClass = await _classService.AddClass(classdto);
                return Ok(newClass);
            }
            catch (ClassAlreadyExistsException ex)
            {
                return Conflict(new ErrorModel { ErrorCode = StatusCodes.Status409Conflict, ErrorMessage = ex.Message });
            }
            catch (NoSuchCourseOfferingException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a class from the database.
        /// </summary>
        /// <param name="classId"> The Id of the class to be deleted.</param>
        /// <returns></returns>
        [HttpDelete("DeleteClass")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(ClassReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassReturnDTO>> DeleteClass(int classId)
        {
            try
            {
                var deletedClass = await _classService.DeleteClass(classId);
                return Ok(deletedClass);
            }
            catch (NoSuchClassException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Gets the details of a class.
        /// </summary>
        /// <param name="classId"> The Id of the class for which the details are to be fetched.</param>
        /// <returns></returns>
        [HttpGet("GetClass")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        [ProducesResponseType(typeof(ClassReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassReturnDTO>> GetClass(int classId)
        {
            try
            {
                var classObj = await _classService.GetClass(classId);
                return Ok(classObj);
            }
            catch (NoSuchClassException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Gets all the classes in the database.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetClasses")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        [ProducesResponseType(typeof(ClassReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ClassReturnDTO>>> GetClasses()
        {
            try
            {
                var classes = await _classService.GetClasses();
                return Ok(classes);
            }
            catch (NoClassFoundException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Updates the timeing of a class.
        /// </summary>
        /// <param name="updateclassdto"> UpdateClassDTO object containing the details of the class to be updated.</param>
        /// <returns></returns>
        [HttpPut("UpdateClassTime")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(ClassReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassReturnDTO>> UpdateClassTime(UpdateClassDTO updateclassdto)
        {
            try
            {
                var updatedClass = await _classService.UpdateClassTime(updateclassdto);
                return Ok(updatedClass);
            }
            catch (NoSuchClassException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }
    }
}
