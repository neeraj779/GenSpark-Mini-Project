using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Registers a new student with the provided registration details.
        /// </summary>
        /// <param name="student">The registration details of the student.</param>
        /// <returns>
        /// An action result containing the newly created student if successful.
        /// </returns>
        /// <response code="200">Returns the newly created student.</response>
        [HttpPost("Register")]
        public async Task<ActionResult> AddStudent(StudentRegisterDTO student)
        {
            var newStudent = await _studentService.RegisterStudent(student);
            return Ok(newStudent);
        }

        /// <summary>
        /// Deletes a student based on the provided student ID.
        /// </summary>
        /// <param name="studentId">The ID of the student to be deleted.</param>
        /// <returns>
        /// An action result indicating the success of the deletion operation.
        /// </returns>
        /// <response code="200">If the student is successfully deleted.</response>
        [HttpDelete("DeleteStudent")]
        public async Task<ActionResult> DeleteStudent(int studentId)
        {
            var deletedStudent = await _studentService.DeleteStudent(studentId);
            return Ok(deletedStudent);
        }

        /// <summary>
        /// Updates an existing student with the provided details.
        /// </summary>
        /// <param name="student">The updated details of the student.</param>
        /// <returns>
        /// An action result containing the updated student if successful.
        /// </returns>
        /// <response code="200">Returns the updated student.</response>
        //[HttpPut("UpdateStudent")]
        //public async Task<ActionResult> UpdateStudent(StudentReturnDTO student)
        //{
        //    var updatedStudent = await _studentService.UpdateStudent(student);
        //    return Ok(updatedStudent);
        //}

        /// <summary>
        /// Retrieves a student's details based on the provided student ID.
        /// </summary>
        /// <param name="studentId">The ID of the student to be retrieved.</param>
        /// <returns>
        /// An action result containing the student's details if found.
        /// </returns>
        /// <response code="200">Returns the student's details.</response>
        /// <response code="404">If the student with the given ID is not found.</response>
        [HttpGet("GetStudentById")]
        public async Task<ActionResult> GetStudentById(int studentId)
        {
            var student = await _studentService.GetStudentById(studentId);
            return Ok(student);
        }

        /// <summary>
        /// Retrieves a list of all students.
        /// </summary>
        /// <returns>
        /// An action result containing the list of all students.
        /// </returns>
        /// <response code="200">Returns the list of all students.</response>
        [HttpGet("GetAllStudents")]
        public async Task<ActionResult> GetStudents()
        {
            var students = await _studentService.GetStudents();
            return Ok(students);
        }
    }
}
