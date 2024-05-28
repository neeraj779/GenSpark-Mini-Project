using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models;
using StudentManagementAPI.Models.DTOs;
using StudentManagementAPI.Services;

namespace StudentManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Authenticates a user based on the provided login credentials for JWT Token.
        /// </summary>
        /// <param name="user">The login details of the user.</param>
        /// <returns>
        /// JWT Token if successful, 
        /// or an appropriate error status code if the login fails.
        /// </returns>
        /// <response code="200">Returns the authentication response.</response>
        /// <response code="403">If the user is not active.</response>
        /// <response code="401">If the login credentials are invalid.</response>
        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginDTO user)
        {
            try
            {
                var loginReturn = await _userService.Login(user);
                return Ok(loginReturn);
            }
            catch (UserNotActiveException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
            catch (InvalidLoginException)
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <param name="user">The registration details of the user.</param>
        /// <returns>
        /// An action result containing the newly created user if successful, 
        /// or an appropriate error status code if the registration fails.
        /// </returns>
        /// <response code="200">Returns the newly created user.</response>
        /// <response code="409">If a user with the same details already exists.</response>
        /// <response code="400">If the provided role is invalid or the user is not part of the institution.</response>
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterDTO user)
        {
            try
            {
                var newUser = await _userService.Register(user);
                return Ok(newUser);
            }
            catch (DuplicateUserException ex)
            {
                return Conflict(new ErrorModel { ErrorCode = StatusCodes.Status409Conflict, ErrorMessage = ex.Message });
            }
            catch (InvalidRoleException ex)
            {
                return BadRequest(new ErrorModel { ErrorCode = StatusCodes.Status400BadRequest, ErrorMessage = ex.Message });
            }
            catch (UserNotPartOfInstitutionException ex)
            {
                return BadRequest(new ErrorModel { ErrorCode = StatusCodes.Status400BadRequest, ErrorMessage = ex.Message });
            }
        }
    }
}
