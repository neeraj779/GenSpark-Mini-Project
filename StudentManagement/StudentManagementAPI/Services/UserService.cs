using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository<int, User> _userRepo;
        private readonly IRepository<int, Teacher> _teacherRepo;
        private readonly IRepository<int, Student> _studentRepo;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository<int, User> userRepo,
            IRepository<int, Teacher> teacherRepo,
            IRepository<int, Student> studentRepo,
            ITokenService tokenService,
            ILogger<UserService> logger
            )
        {
            _userRepo = userRepo;
            _teacherRepo = teacherRepo;
            _studentRepo = studentRepo;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<LoginReturnDTO> Login(UserLoginDTO loginDTO)
        {
            var userDB = await _userRepo.GetByUserName(loginDTO.UserName);
            if (userDB == null)
            {
                _logger.LogWarning("Login failed for user: {UserName} - User not found", loginDTO.UserName);
                throw new InvalidLoginException();
            }

            if (userDB.Status != "Active")
            {
                _logger.LogWarning("Login failed for user: {UserName} - User not active", loginDTO.UserName);
                throw new UserNotActiveException();
            }

            HMACSHA512 hMACSHA = new HMACSHA512(userDB.PasswordHashKey);
            var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            bool isPasswordCorrect = encrypterPass.SequenceEqual(userDB.Password);

            if (!isPasswordCorrect)
            {
                _logger.LogWarning("Login failed for user: {UserName} - Incorrect password", loginDTO.UserName);
                throw new InvalidLoginException();
            }
            LoginReturnDTO returnDTO = new LoginReturnDTO
            {
                AccessToken = _tokenService.GenerateToken(userDB),
                TokenType = "Bearer",
                Role = userDB.Role.ToString()
            };
            return returnDTO;
        }

        public async Task<RegisteredUserDTO> Register(UserRegisterDTO user)
        {
            if (!Enum.TryParse(user.Role, out UserRole role))
            {
                _logger.LogWarning("Register failed for user: {UserName} - Invalid role", user.UserName);
                throw new InvalidRoleException();
            }

            var userDB = await _userRepo.GetByUserName(user.UserName);
            if (userDB != null)
            {
                _logger.LogWarning("Register failed for user: {UserName} - Duplicate username", user.UserName);
                throw new DuplicateUserNameException();
            }

            switch (role)
            {
                case UserRole.Teacher:
                    var teacher = await _teacherRepo.Get(user.AccountId);
                    if (teacher == null)
                        throw new UserNotPartOfInstitutionException();

                    if (teacher.UserId != null)
                        throw new DuplicateUserException();

                    User newUserTeacher = await CreateUser(user, role);
                    teacher.UserId = newUserTeacher.UserId;
                    await _teacherRepo.Update(teacher);
                    return MapUserToReturnDTO(newUserTeacher);

                case UserRole.Student:
                    var student = await _studentRepo.Get(user.AccountId);
                    if (student == null)
                    {
                        _logger.LogWarning("Register failed for user: {UserName} - User not part of institution", user.UserName);
                        throw new UserNotPartOfInstitutionException();
                    }
                    if (student.UserId != null)
                    {
                        _logger.LogWarning("Register failed for user: {UserName} - Duplicate user", user.UserName);
                        throw new DuplicateUserException();
                    }

                    User newUserStudent = await CreateUser(user, role);
                    student.UserId = newUserStudent.UserId;
                    await _studentRepo.Update(student);
                    return MapUserToReturnDTO(newUserStudent);
                default:
                    _logger.LogWarning("Register failed for user: {UserName} - Invalid role", user.UserName);
                    throw new InvalidRoleException();
            }
        }

        private async Task<User> CreateUser(UserRegisterDTO user, UserRole role)
        {
            User newUser = new User();

            HMACSHA512 hMACSHA = new HMACSHA512();
            newUser.UserName = user.UserName;
            newUser.PasswordHashKey = hMACSHA.Key;
            newUser.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
            newUser.Status = "Inactive";
            newUser.Role = role;
            newUser.RegistrationDate = DateTime.Now;

            await _userRepo.Add(newUser);

            return newUser;
        }

        public async Task<IEnumerable<RegisteredUserDTO>> GetAllUsers()
        {
            var users = await _userRepo.Get();

            if (users.Count() == 0)
            {
                _logger.LogWarning("No users found");
                throw new NoUserFoundException();
            }

            List<RegisteredUserDTO> userReturnDTOs = new List<RegisteredUserDTO>();
            foreach (var user in users)
                userReturnDTOs.Add(MapUserToReturnDTO(user));

            return userReturnDTOs;
        }

        public async Task<RegisteredUserDTO> ActivateUser(int id)
        {
            var user = await _userRepo.Get(id);

            if (user == null)
            {
                _logger.LogWarning("Activate failed for user: {UserId} - User not found", id);
                throw new NoSuchUserException();
            }

            user.Status = "Active";
            await _userRepo.Update(user);

            return MapUserToReturnDTO(user);
        }

        public async Task<RegisteredUserDTO> DeactivateUser(int id)
        {
            var user = await _userRepo.Get(id);

            if (user == null)
            {
                _logger.LogWarning("Deactivate failed for user: {UserId} - User not found", id);
                throw new NoSuchUserException();
            }

            user.Status = "Inactive";
            await _userRepo.Update(user);

            return MapUserToReturnDTO(user);
        }

        public RegisteredUserDTO MapUserToReturnDTO(User user)
        {
            RegisteredUserDTO registeredUserDTO = new RegisteredUserDTO
            {
                AccountId = user.UserId,
                UserName = user.UserName,
                Role = user.Role.ToString(),
                Status = user.Status,
                RegistrationDate = user.RegistrationDate
            };
            return registeredUserDTO;
        }
    }
}
