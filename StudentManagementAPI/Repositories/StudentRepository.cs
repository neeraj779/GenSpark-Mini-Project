using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;

namespace StudentManagementAPI.Repositories
{
    public class StudentRepository : IRepository<int, Student>
    {
        private readonly StudentManagementContext _context;

        public StudentRepository(StudentManagementContext context)
        {
            _context = context;
        }
        public async Task<Student> Add(Student item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException)
            {
                throw new UnableToAddException("Unable to add student. Please check the data and try again.");
            }
        }

        public async Task<Student> Delete(int key)
        {
            var student = await Get(key);
            if (student != null)
            {
                _context.Remove(student);
                await _context.SaveChangesAsync();
                return student;
            }
            throw new NoSuchStudentException();
        }

        public async Task<Student> Get(int key)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == key);
            return student;
        }

        public async Task<IEnumerable<Student>> Get()
        {
            var student = await _context.Students.ToListAsync();
            return student;
        }

        public async Task<Student> Update(Student item)
        {
            var student = await Get(item.StudentId);
            if (student != null)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchStudentException();
        }
    }
}
