using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;

namespace PizzaAPI.Repositories
{
    public class UserRepository : IUserRepository<int, User>
    {
        private readonly StudentManagementContext _context;

        public UserRepository(StudentManagementContext context)
        {
            _context = context;
        }
        public async Task<User> Add(User item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException)
            {
                throw new UnableToAddException("Unable to add user. Please check the data and try again.");
            }
        }

        public async Task<User> Delete(int key)
        {
            var user = await Get(key);
            if (user != null)
            {
                _context.Remove(user);
                await _context.SaveChangesAsync();
                return user;
            }
            throw new NoSuchUserException();
        }

        public async Task<User> Get(int key)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == key);
            if (user == null)
            {
                throw new NoSuchUserException();
            }
            return user;
        }

        public async Task<User> GetByUserName(string userName)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                throw new NoSuchUserException();
            }
            return user;
        }

        public async Task<IEnumerable<User>> Get()
        {
            var users =  await _context.Users.ToListAsync();
            if (users == null)
            {
                throw new NoUserFoundException();
            }
            return users;
        }

        public async Task<User> Update(User item)
        {
            var user = await Get(item.UserId);
            if (user != null)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return user;
            }
            throw new NoSuchUserException();
        }
    }
}
