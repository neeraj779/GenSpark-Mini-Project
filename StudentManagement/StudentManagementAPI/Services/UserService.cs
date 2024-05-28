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

        public UserService(
            IUserRepository<int, User> userRepo,
            IRepository<int, Teacher> teacherRepo,
            IRepository<int, Student> studentRepo,
            ITokenService tokenService
            ) 
        {
            _userRepo = userRepo;
            _teacherRepo = teacherRepo;
            _studentRepo = studentRepo;
            _tokenService = tokenService;
        }

        public async Task<LoginReturnDTO> Login(UserLoginDTO loginDTO)
        {
            var userDB = await _userRepo.GetByUserName(loginDTO.UserName);
            if (userDB == null)
            {
                throw new UnauthorizedUserException();
            }

            if(userDB.Status != "Active")
            {
                throw new UserNotActiveException();
            }

            HMACSHA512 hMACSHA = new HMACSHA512(userDB.PasswordHashKey);
            var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            bool isPasswordCorrect = encrypterPass.SequenceEqual(userDB.Password);

            if (!isPasswordCorrect)
            {
                throw new UnauthorizedUserException();
            }
            LoginReturnDTO returnDTO = new LoginReturnDTO();
            returnDTO.AccessToken = _tokenService.GenerateToken(userDB);
            returnDTO.TokenType = "Bearer";
            return returnDTO;
        }

        public async Task<RegisteredUserDTO> Register(UserRegisterDTO user)
        {
            var teacherDB = await _teacherRepo.Get(user.Id);
            var studentDB = await _studentRepo.Get(user.Id);

            if (teacherDB == null && studentDB == null)
            {
                throw new UserNotPartOfInstitutionException();
            }

            if ((teacherDB != null && teacherDB.UserId != null) || (studentDB != null && studentDB.UserId != null))
            {
                throw new DuplicateUserException();
            }

            User newUser = new User();

            HMACSHA512 hMACSHA = new HMACSHA512();
            newUser.UserName = user.UserName;
            newUser.PasswordHashKey = hMACSHA.Key;
            newUser.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
            newUser.Status = "Inactive";
            if (Enum.TryParse<UserRole>(user.Role, out UserRole role))
            {
                newUser.Role = role;
            }
            else
            {
                throw new InvalidRoleException(); 
            }
            newUser.RegistrationDate = DateTime.Now;

            await _userRepo.Add(newUser);

            if (studentDB == null)
            {
                var teacher = await _teacherRepo.Get(user.Id);
                teacher.UserId = newUser.UserId; 
                await _teacherRepo.Update(teacher); 
            }
            else if (teacherDB == null)
            {
                var student = await _studentRepo.Get(user.Id);
                student.UserId = newUser.UserId; 
                await _studentRepo.Update(student); 
            }

            return MapUserToReturnDTO(newUser);
        }

        public RegisteredUserDTO MapUserToReturnDTO(User user)
        {
            RegisteredUserDTO registeredUserDTO = new RegisteredUserDTO();
            registeredUserDTO.UserName = user.UserName;
            registeredUserDTO.Role = user.Role.ToString();
            registeredUserDTO.RegistrationDate = user.RegistrationDate;
            return registeredUserDTO;
        }
    }
}
