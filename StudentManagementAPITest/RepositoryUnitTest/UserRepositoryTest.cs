using Microsoft.EntityFrameworkCore;
using Moq;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagementAPITest.RepositoryUnitTest
{
    public class UserRepositoryTest
    {
        StudentManagementContext context;
        private Mock<StudentManagementContext> mockContext;
        private UserRepository mockUserRepository;



        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            mockContext = new Mock<StudentManagementContext>(optionsBuilder.Options);
            mockUserRepository = new UserRepository(mockContext.Object);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }


        [Test]
        public async Task TestAddUser()
        {
            //Arrange 
            IUserRepository<int, User> userRepository = new UserRepository(context);

            var newUser = new User
            {
                UserId = 101,
                UserName = "newUser",
                Status = "Active",
                Role = UserRole.Admin,
                RegistrationDate = DateTime.UtcNow
            };

            HMACSHA512 hMACSHA = new HMACSHA512();
            newUser.PasswordHashKey = hMACSHA.Key;
            newUser.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes("test"));

            //Action
            await userRepository.Add(newUser);

            //Assert
            var userResult = await userRepository.Get(101);
            Assert.That(userResult.UserId, Is.EqualTo(101));
        }


        [Test]
        public void Add_WhenDbUpdateExceptionThrown_ShouldThrowUnableToAddException()
        {
            // Arrange
            var newUser = new User
            {
                UserId = 101,
                UserName = "newUser",
                Status = "Active",
                Role = UserRole.Admin,
                RegistrationDate = DateTime.UtcNow
            };

            //Action
            mockContext.Setup(c => c.Add(newUser)).Verifiable();
            mockContext.Setup(c => c.SaveChangesAsync(default)).ThrowsAsync(new DbUpdateException());

            // Assert
            var exception = Assert.ThrowsAsync<UnableToAddException>(async () => await mockUserRepository.Add(newUser));
            Assert.That(exception.Message, Is.EqualTo("Unable to add user. Please check the data and try again."));
        }

        [Test]
        public async Task TestGetAllUsers()
        {
            //Arrange 
            IUserRepository<int, User> userRepository = new UserRepository(context);

            //Action
            var result = await userRepository.Get();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task TestGetUserById()
        {
            //Arrange 
            IUserRepository<int, User> userRepository = new UserRepository(context);

            //Action
            var result = await userRepository.Get(100);

            //Assert
            Assert.That(result.UserId, Is.EqualTo(100));
        }

        [Test]
        public async Task TestGetUserByUserName()
        {
            //Arrange 
            IUserRepository<int, User> userRepository = new UserRepository(context);

            //Action
            var result = await userRepository.GetByUserName("admin");

            //Assert
            Assert.That(result.UserName, Is.EqualTo("admin"));
        }

        [Test]
        public async Task TestUpdateUser()
        {
            //Arrange 
            IUserRepository<int, User> userRepository = new UserRepository(context);

            var user = await userRepository.Get(100);
            user.UserName = "test1";

            //Action
            await userRepository.Update(user);

            //Assert
            var result = await userRepository.Get(100);
            Assert.That(result.UserName, Is.EqualTo("test1"));
        }

        [Test]
        public void TestUpdateFail()
        {
            //Arrange 
            IUserRepository<int, User> userRepository = new UserRepository(context);

            var user = new User
            {
                UserId = 10,
                UserName = "test1",
                Status = "Active",
                Role = UserRole.Admin,
                RegistrationDate = DateTime.UtcNow
            };

            //Action
            var ex = Assert.ThrowsAsync<NoSuchUserException>(() => userRepository.Update(user));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such user found!"));
        }

        [Test]
        public async Task TestDeleteUser()
        {
            //Arrange 
            IUserRepository<int, User> userRepository = new UserRepository(context);

            //Action
            await userRepository.Delete(100);

            //Assert
            var result = await userRepository.Get(100);
            Assert.That(result, Is.Null);
        }


        [Test]
        public void TestDeleteFail()
        {
            //Arrange 
            IUserRepository<int, User> userRepository = new UserRepository(context);

            //Action
            var ex = Assert.ThrowsAsync<NoSuchUserException>(() => userRepository.Delete(10));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such user found!"));
        }
    }
}