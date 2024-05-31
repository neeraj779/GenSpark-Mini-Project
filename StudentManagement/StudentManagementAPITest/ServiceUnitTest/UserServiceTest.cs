using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;
using StudentManagementAPI.Repositories;
using StudentManagementAPI.Services;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Moq;

namespace StudentManagementAPITest.ServiceUnitTest
{
    public class UserServiceTests
    {
        StudentManagementContext context;
        private IUserRepository<int, User> userRepo;
        private IRepository<int, Teacher> teacherRepo;
        private IRepository<int, Student> studentRepo;
        private Mock<ITokenService> tokenServiceMock;
        private Mock<ILogger<UserService>> loggerMock;
        private UserService userService;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            userRepo = new UserRepository(context);
            teacherRepo = new TeacherRepository(context);
            studentRepo = new StudentRepository(context);

            tokenServiceMock = new Mock<ITokenService>();
            loggerMock = new Mock<ILogger<UserService>>();

            userService = new UserService(
                userRepo,
                teacherRepo,
                studentRepo,
                tokenServiceMock.Object,
                loggerMock.Object
            );
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsLoginReturnDTO()
        {
            // Arrange
            var loginDTO = new UserLoginDTO { UserName = "testuser", Password = "password" };
            var passwordHash = new HMACSHA512();
            var userDB = new User
            {
                UserName = "testuser",
                PasswordHashKey = passwordHash.Key,
                Password = passwordHash.ComputeHash(Encoding.UTF8.GetBytes("password")),
                Status = "Active",
                Role = UserRole.Student
            };

            context.Users.Add(userDB);
            await context.SaveChangesAsync();

            tokenServiceMock.Setup(service => service.GenerateToken(It.IsAny<User>()))
                             .Returns("token");

            // Action
            var result = await userService.Login(loginDTO);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.AccessToken, Is.EqualTo("token"));
            Assert.That(result.TokenType, Is.EqualTo("Bearer"));
            Assert.That(result.Role, Is.EqualTo("Student"));
        }

        [Test]
        public void Login_UserNotFound_ThrowsInvalidLoginException()
        {
            // Arrange
            var loginDTO = new UserLoginDTO { UserName = "nonexistentuser", Password = "password" };
            var wrongPasswordDTO = new UserLoginDTO { UserName = "admin", Password = "wrongpassword" };


            // Action
            var ex  =Assert.ThrowsAsync<InvalidLoginException>(() => userService.Login(loginDTO));
            var ex2 = Assert.ThrowsAsync<InvalidLoginException>(() => userService.Login(wrongPasswordDTO));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Username or password is incorrect!"));
            Assert.That(ex2.Message, Is.EqualTo("Username or password is incorrect!"));
        }

        [Test]
        public void Login_UserNotActive_ThrowsUserNotActiveException()
        {
            // Arrange
            var loginDTO = new UserLoginDTO { UserName = "inactiveuser", Password = "password" };
            var userDB = new User
            {
                UserName = "inactiveuser",
                PasswordHashKey = new byte[64],
                Password = new byte[64],
                Status = "Inactive",
                Role = UserRole.Student
            };

            context.Users.Add(userDB);
            context.SaveChanges();

            // Action
            var ex = Assert.ThrowsAsync<UserNotActiveException>(() => userService.Login(loginDTO));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! User is not active please contact admin!"));

        }

        [Test]
        public async Task Register_ValidRole_ReturnsRegisteredUserDTO()
        {
            // Arrange
            var userRegisterDTO = new UserRegisterDTO { UserName = "newuser", Password = "password", Role = "Teacher", AccountId = 1 };
            var userRegisterDTO2 = new UserRegisterDTO { UserName = "newuser2", Password = "password", Role = "Student", AccountId = 2 };
            var teacher = new Teacher { TeacherId = 1, UserId = null };
            var student = new Student { StudentId = 2, UserId = null };

            context.Teachers.Add(teacher);
            context.Students.Add(student);
            await context.SaveChangesAsync();

            // Act
            var result = await userService.Register(userRegisterDTO);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.UserName, Is.EqualTo(userRegisterDTO.UserName));
            Assert.That(result.Role, Is.EqualTo(userRegisterDTO.Role));
        }

        [Test]
        public void Register_InvalidRole_ThrowsInvalidRoleException()
        {
            // Arrange
            var userRegisterDTO = new UserRegisterDTO { UserName = "newuser", Password = "password", Role = "InvalidRole", AccountId = 1 };

            // Action
            var ex = Assert.ThrowsAsync<InvalidRoleException>(() => userService.Register(userRegisterDTO));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Invalid role!"));
        }

        [Test]
        public async Task Register_DuplicateUserName_ThrowsDuplicateUserNameException()
        {
            // Arrange
            var userRegisterDTO = new UserRegisterDTO { UserName = "existinguser", Password = "password", Role = "Teacher", AccountId = 1 };
            var existingUser = new User { UserName = "existinguser" };

            context.Users.Add(existingUser);
            await context.SaveChangesAsync();

            // Action
            var ex = Assert.ThrowsAsync<DuplicateUserNameException>(() => userService.Register(userRegisterDTO));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Opps! The username is already taken. please try another one."));
        }

        [Test]
        public void Register_UserNotPartOfInstitution_ThrowsUserNotPartOfInstitutionException()
        {
            // Arrange
            var userRegisterDTO = new UserRegisterDTO { UserName = "newuser", Password = "password", Role = "Teacher", AccountId = 1 };
            var userRegisterDTO2 = new UserRegisterDTO { UserName = "newuser2", Password = "password", Role = "Student", AccountId = 2 };

            // Action
            var ex = Assert.ThrowsAsync<UserNotPartOfInstitutionException>(() => userService.Register(userRegisterDTO));
            var ex2 = Assert.ThrowsAsync<UserNotPartOfInstitutionException>(() => userService.Register(userRegisterDTO2));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("User is not part of the institution!"));
            Assert.That(ex2.Message, Is.EqualTo("User is not part of the institution!"));
        }

        [Test]
        public async Task Register_DuplicateUser_ThrowsDuplicateUserException()
        {
            // Arrange
            var userRegisterDTO = new UserRegisterDTO { UserName = "testusername", Password = "password", Role = "Teacher", AccountId = 2000 };
            var userRegisterDTO2 = new UserRegisterDTO { UserName = "testusername2", Password = "password", Role = "Student", AccountId = 4000 };

            await userService.Register(userRegisterDTO);
            await userService.Register(userRegisterDTO2);

            userRegisterDTO.UserName = "testusername10";
            userRegisterDTO2.UserName = "testusername20";

            // Action
            var ex = Assert.ThrowsAsync<DuplicateUserException>(() => userService.Register(userRegisterDTO));
            var ex2 = Assert.ThrowsAsync<DuplicateUserException>(() => userService.Register(userRegisterDTO2));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("The user already has an associated account."));
            Assert.That(ex2.Message, Is.EqualTo("The user already has an associated account."));
        }

        [Test]
        public async Task GetAllUsers_ReturnsIEnumerableRegisteredUserDTO()
        {
            // Arrange
            var userRegisterDTO = new UserRegisterDTO { UserName = "newuser", Password = "password", Role = "Teacher", AccountId = 2000 };
            var userRegisterDTO2 = new UserRegisterDTO { UserName = "newuser2", Password = "password", Role = "Student", AccountId = 4000 };

            await userService.Register(userRegisterDTO);
            await userService.Register(userRegisterDTO2);

            // Action
            var result = await userService.GetAllUsers();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task GetAllUsers_NoUsers_ReturnsEmptyIEnumerableRegisteredUserDTO()
        {
            // Arrange
            context.Users.RemoveRange(context.Users);
            await context.SaveChangesAsync();

            // Action
            var ex = Assert.ThrowsAsync<NoUserFoundException>(() => userService.GetAllUsers());

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No user found!"));
        }

        [Test]
        public async Task ActivateUser_ValidUserName_ReturnsRegisteredUserDTO()
        {
            // Arrange
            var userRegisterDTO = new UserRegisterDTO { UserName = "newuser", Password = "password", Role = "Teacher", AccountId = 2000 };
            var user = await userService.Register(userRegisterDTO);

            // Action
            var result = await userService.ActivateUser(user.AccountId);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.UserName, Is.EqualTo(user.UserName));
            Assert.That(result.Status, Is.EqualTo("Active"));
        }

        [Test]
        public void ActivateUser_InvalidUserId_ThrowsUserNotFoundException()
        {
            // Arrange
            var invalidUserId = 1000;

            // Action
            var ex = Assert.ThrowsAsync<NoSuchUserException>(() => userService.ActivateUser(invalidUserId));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such user found!"));
        }

        [Test]
        public async Task DeactivateUser_ValidUserName_ReturnsRegisteredUserDTO()
        {
            //Arrange
            int accountId = 100;


            // Action
            var result = await userService.DeactivateUser(100);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Status, Is.EqualTo("Inactive"));
        }

        [Test]
        public void DeactivateUser_InvalidUserId_ThrowsUserNotFoundException()
        {
            // Arrange
            var invalidUserId = 1000;

            // Action
            var ex = Assert.ThrowsAsync<NoSuchUserException>(() => userService.DeactivateUser(invalidUserId));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such user found!"));
        }
    }
}
