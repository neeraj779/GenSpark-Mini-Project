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
                _logger.LogWarning("Login failed: User not found");
                throw new InvalidLoginException();
            }

            if (userDB.Status != "Active")
            {
                _logger.LogWarning("Login failed: User not active");
                throw new UserNotActiveException();
            }

            if (!IsPasswordCorrect(loginDTO.Password, userDB.PasswordHashKey, userDB.Password))
            {
                _logger.LogWarning("Login failed: Invalid password");
                throw new InvalidLoginException();
            }

            return new LoginReturnDTO
            {
                AccessToken = _tokenService.GenerateToken(userDB),
                TokenType = "Bearer",
                Role = userDB.Role.ToString()
            };
        }

        public async Task<RegisteredUserDTO> Register(UserRegisterDTO user)
        {
            if (!Enum.TryParse(user.Role, out UserRole role))
            {
                _logger.LogWarning("Registration failed: Invalid role");
                throw new InvalidRoleException();
            }

            var userDB = await _userRepo.GetByUserName(user.UserName);
            if (userDB != null)
            {
                _logger.LogWarning("Registration failed: Duplicate username");
                throw new DuplicateUserNameException();
            }

            switch (role)
            {
                case UserRole.Teacher:
                    return await RegisterTeacher(user);

                case UserRole.Student:
                    return await RegisterStudent(user);

                default:
                    _logger.LogWarning("Register failed: Invalid role");
                    throw new InvalidRoleException();
            }
        }

        private async Task<RegisteredUserDTO> RegisterTeacher(UserRegisterDTO user)
        {
            var teacher = await _teacherRepo.Get(user.AccountId);
            ValidateUserPartOfInstitution(teacher);

            User newUserTeacher = await CreateUser(user, UserRole.Teacher);
            teacher.UserId = newUserTeacher.UserId;
            await _teacherRepo.Update(teacher);

            return MapUserToReturnDTO(newUserTeacher);
        }

        private async Task<RegisteredUserDTO> RegisterStudent(UserRegisterDTO user)
        {
            var student = await _studentRepo.Get(user.AccountId);
            ValidateUserPartOfInstitution(student);

            User newUserStudent = await CreateUser(user, UserRole.Student);
            student.UserId = newUserStudent.UserId;
            await _studentRepo.Update(student);

            return MapUserToReturnDTO(newUserStudent);
        }

        private void ValidateUserPartOfInstitution(object institutionMember)
        {
            if (institutionMember == null)
            {
                _logger.LogWarning("Registration failed: User not part of institution");
                throw new UserNotPartOfInstitutionException();
            }

            bool isTeacherWithUserId = institutionMember is Teacher teacher && teacher.UserId != null;
            bool isStudentWithUserId = institutionMember is Student student && student.UserId != null;

            if (isTeacherWithUserId || isStudentWithUserId)
            {
                _logger.LogWarning("Registration failed: Duplicate user");
                throw new DuplicateUserException();
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
                _logger.LogWarning("Activation failed: User not found");
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
                _logger.LogWarning("Deactivation failed: User not found");
                throw new NoSuchUserException();
            }

            user.Status = "Inactive";
            await _userRepo.Update(user);

            return MapUserToReturnDTO(user);
        }

        private bool IsPasswordCorrect(string password, byte[] passwordHashKey, byte[] storedPasswordHash)
        {
            using var hmac = new HMACSHA512(passwordHashKey);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedPasswordHash);
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
