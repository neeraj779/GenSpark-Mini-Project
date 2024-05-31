using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;

namespace StudentManagementAPI.Repositories
{
    public class CourseOfferingRepository : IRepository<int, CourseOffering>
    {
        public readonly StudentManagementContext _context;

        public CourseOfferingRepository(StudentManagementContext context)
        {
            _context = context;
        }

        public async Task<CourseOffering> Add(CourseOffering item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException)
            {
                throw new UnableToAddException("Unable to add course offering. Please check the data and try again.");
            }
        }

        public async Task<CourseOffering> Get(int key)
        {
            var courseOffering = await _context.CourseOfferings.SingleOrDefaultAsync(c => c.CourseOfferingId == key);
            return courseOffering;
        }

        public async Task<CourseOffering> Delete(int key)
        {
            var courseOffering = await Get(key);
            if (courseOffering != null)
            {
                _context.CourseOfferings.Remove(courseOffering);
                await _context.SaveChangesAsync();
                return courseOffering;
            }
            throw new NoSuchCourseOfferingException();
        }

        public async Task<IEnumerable<CourseOffering>> Get()
        {
            var courseOfferings = await _context.CourseOfferings.ToListAsync();
            return courseOfferings;
        }

        public async Task<CourseOffering> Update(CourseOffering item)
        {
            var courseOffering = await Get(item.CourseOfferingId);
            if (courseOffering != null)
            {
                _context.CourseOfferings.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchCourseOfferingException();
        }
    }
}
