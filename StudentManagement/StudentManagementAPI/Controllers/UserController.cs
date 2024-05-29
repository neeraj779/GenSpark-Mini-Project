using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models;
using StudentManagementAPI.Models.DTOs;
using StudentManagementAPI.Services;

namespace StudentManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
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
        [HttpPost("Login")]
        [ProducesResponseType(typeof(LoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginReturnDTO>> Login(UserLoginDTO user)
        {
            try
            {
                var loginReturn = await _userService.Login(user);
                return Ok(loginReturn);
            }
            catch (UserNotActiveException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorModel { ErrorCode = StatusCodes.Status403Forbidden, ErrorMessage = ex.Message });
            }
            catch (InvalidLoginException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new ErrorModel { ErrorCode = StatusCodes.Status401Unauthorized, ErrorMessage = ex.Message });

            }
        }

        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <param name="user">The registration details of the user.</param>
        [HttpPost("RegisterAccount")]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<RegisteredUserDTO>> Register(UserRegisterDTO user)
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

            catch(DuplicateUserNameException ex)
            {
                return Conflict(new ErrorModel { ErrorCode = StatusCodes.Status409Conflict, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>List of registered user DTOs</returns>
        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(RegisteredUserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RegisteredUserDTO>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }

            catch (NoUserFoundException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Activates a user.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Action result</returns>
        [HttpPost("ActivateUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ActivateUser(int id)
        {
            try
            {
                var user = await _userService.ActivateUser(id);
                return Ok(user);
            }
            catch (NoSuchUserException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Deactivates a user.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Action result</returns>
        [HttpPost("DeactivateUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeactivateUser(int id)
        {
            try
            {
                var user = await _userService.DeactivateUser(id);
                return Ok(user);
            }
            catch (NoSuchUserException ex)
            {
                return NotFound(new ErrorModel { ErrorCode = StatusCodes.Status404NotFound, ErrorMessage = ex.Message });
            }
        }

    }
}
