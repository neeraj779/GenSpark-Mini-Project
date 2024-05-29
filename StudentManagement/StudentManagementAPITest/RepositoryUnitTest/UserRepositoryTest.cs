using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Repositories;
using StudentManagementAPI.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagementAPITest.RepositoryUnitTest
{
    public class UserRepositoryTest
    {
        StudentManagementContext context;
        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            var user = new User
            {
                UserId = 1,
                UserName = "test",
                Status = "Active",
                Role = UserRole.Admin,
                RegistrationDate = DateTime.UtcNow
            };

            HMACSHA512 hMACSHA = new HMACSHA512();
            user.PasswordHashKey = hMACSHA.Key;
            user.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes("password"));

            context.Users.Add(user);
        }


        [Test]
        public async Task TestAddUser()
        {
            //Arrange 
            IUserRepository<int, User> userRepository = new UserRepository(context);

            var newUser = new User
            {
                UserId = 2,
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
            var userResult = await userRepository.Get(1);
            Assert.That(userResult.UserId, Is.EqualTo(1));
        }

        [Test]
        public async Task TestGetAllUsers()
        {
            //Arrange 
            IUserRepository<int, User> userRepository = new UserRepository(context);

            //Action
            var result = await userRepository.Get();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task TestGetUserById()
        {
            //Arrange 
            IUserRepository<int, User> userRepository = new UserRepository(context);

            //Action
            var result = await userRepository.Get(1);

            //Assert
            Assert.That(result.UserId, Is.EqualTo(1));
        }

        [Test]
        public async Task TestGetUserByUserName()
        {
            //Arrange 
            IUserRepository<int, User> userRepository = new UserRepository(context);

            //Action
            var result = await userRepository.GetByUserName("test");

            //Assert
            Assert.That(result.UserName, Is.EqualTo("test"));
        }

        [Test]
        public async Task TestUpdateUser()
        {
            //Arrange 
            IUserRepository<int, User> userRepository = new UserRepository(context);

            var user = await userRepository.Get(1);
            user.UserName = "test1";

            //Action
            await userRepository.Update(user);

            //Assert
            var result = await userRepository.Get(1);
            Assert.That(result.UserName, Is.EqualTo("test1"));
        }


        [Test]
        public async Task TestDeleteFail()
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