using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("AddClass")]
        [Authorize(Roles = "Admin, Teacher")]
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

        [HttpDelete("DeleteClass")]
        [Authorize(Roles = "Admin, Teacher")]
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

        [HttpGet("GetClass")]
        [Authorize(Roles = "Admin, Teacher, Student")]
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

        [HttpGet("GetClasses")]
        [Authorize(Roles = "Admin, Teacher, Student")]
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

        [HttpPut("UpdateClassTime")]
        [Authorize(Roles = "Admin, Teacher")]
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
