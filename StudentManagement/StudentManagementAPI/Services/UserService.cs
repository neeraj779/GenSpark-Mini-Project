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
            if (!Enum.TryParse(user.Role, out UserRole role))
            {
                throw new InvalidRoleException();
            }

            var userDB = await _userRepo.GetByUserName(user.UserName);
            if (userDB != null)
            {
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
                        throw new UserNotPartOfInstitutionException();
                    }
                    if (student.UserId != null)
                    {
                        throw new DuplicateUserException();
                    }

                    User newUserStudent = await CreateUser(user, role);
                    student.UserId = newUserStudent.UserId;
                    await _studentRepo.Update(student);
                    return MapUserToReturnDTO(newUserStudent);
                default:
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
            List<RegisteredUserDTO> userReturnDTOs = new List<RegisteredUserDTO>();
            foreach (var user in users)
                userReturnDTOs.Add(MapUserToReturnDTO(user));

            return userReturnDTOs;
        }

        public async Task<RegisteredUserDTO> ActivateUser(int id)
        {
            var user = await _userRepo.Get(id);

            user.Status = "Active";
            await _userRepo.Update(user);

            return MapUserToReturnDTO(user);
        }

        public async Task<RegisteredUserDTO> DeactivateUser(int id)
        {
            var user = await _userRepo.Get(id);

            user.Status = "Inactive";
            await _userRepo.Update(user);

            return MapUserToReturnDTO(user);
        }

        public RegisteredUserDTO MapUserToReturnDTO(User user)
        {
            RegisteredUserDTO registeredUserDTO = new RegisteredUserDTO();
            registeredUserDTO.AccountId = user.UserId;
            registeredUserDTO.UserName = user.UserName;
            registeredUserDTO.Role = user.Role.ToString();
            registeredUserDTO.Status = user.Status;
            registeredUserDTO.RegistrationDate = user.RegistrationDate;
            return registeredUserDTO;
        }
    }
}
