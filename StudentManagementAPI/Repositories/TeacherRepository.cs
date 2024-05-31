using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;

namespace StudentManagementAPI.Repositories
{
    public class TeacherRepository : IRepository<int, Teacher>
    {
        private readonly StudentManagementContext _context;

        public TeacherRepository(StudentManagementContext context)
        {
            _context = context;
        }

        public async Task<Teacher> Add(Teacher item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException)
            {
                throw new UnableToAddException("Unable to Register Teacher. Please check the data and try again.");
            }
        }

        public async Task<Teacher> Delete(int key)
        {
            var teacher = await Get(key);
            if (teacher != null)
            {
                _context.Remove(teacher);
                await _context.SaveChangesAsync();
                return teacher;
            }
            throw new NoSuchTeacherException();
        }

        public async Task<Teacher> Get(int key)
        {
            var teacher = await _context.Teachers.SingleOrDefaultAsync(t => t.TeacherId == key);
            return teacher;
        }

        public async Task<IEnumerable<Teacher>> Get()
        {
            var teachers = await _context.Teachers.ToListAsync();
            return teachers;
        }

        public async Task<Teacher> Update(Teacher item)
        {
            var teacher = await Get(item.TeacherId);
            if (teacher != null)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchTeacherException();
        }
    }
}
