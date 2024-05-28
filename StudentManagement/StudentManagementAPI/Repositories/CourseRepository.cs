using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;

namespace StudentManagementAPI.Repositories
{
    public class CourseRepository : IRepository<string, Course>
    {
        private readonly StudentManagementContext _context;

        public CourseRepository(StudentManagementContext context)
        {
            _context = context;
        }

        public async Task<Course> Add(Course item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException)
            {
                throw new UnableToAddException("Unable to add course. Please check the data and try again.");
            }
        }

        public async Task<Course> Get(string key)
        {
            var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseCode == key);
            return course;
        }

        public async Task<Course> Delete(string key)
        {
            var course = await Get(key);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                return course;
            }
            throw new NoSuchCourseException();
        }

        public async Task<IEnumerable<Course>> Get()
        {
            var courses = await _context.Courses.ToListAsync();
            return courses;
        }

        public async Task<Course> Update(Course item)
        {
            var course = await Get(item.CourseCode);
            if (course != null)
            {
                _context.Courses.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchCourseException();
        }
    }
}
