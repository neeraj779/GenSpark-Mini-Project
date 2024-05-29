using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;

namespace StudentManagementAPI.Repositories
{
    public class ClassRepository : IRepository<int, Class>
    {
        public readonly StudentManagementContext _context;

        public ClassRepository(StudentManagementContext context)
        {
            _context = context;
        }

        public async Task<Class> Add(Class item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException)
            {
                throw new UnableToAddException("Unable to add class. Please check the data and try again.");
            }
        }

        public async Task<Class> Get(int key)
        {
            var classObj = await _context.Classes.SingleOrDefaultAsync(c => c.ClassId == key);
            return classObj;
        }

        public async Task<Class> Delete(int key)
        {
            var classObj = await Get(key);
            if (classObj != null)
            {
                _context.Classes.Remove(classObj);
                await _context.SaveChangesAsync();
                return classObj;
            }
            throw new NoSuchClassException();
        }

        public async Task<IEnumerable<Class>> Get()
        {
            var classes = await _context.Classes.ToListAsync();
            return classes;
        }

        public async Task<Class> Update(Class item)
        {
            var classObj = await Get(item.ClassId);
            if (classObj != null)
            {
                _context.Classes.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchClassException();
        }
    }
}
