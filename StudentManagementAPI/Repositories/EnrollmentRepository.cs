using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;

namespace StudentManagementAPI.Repositories
{
    public class EnrollmentRepository : IRepository<int, Enrollment>
    {
        private readonly StudentManagementContext _context;

        public EnrollmentRepository(StudentManagementContext context)
        {
            _context = context;
        }

        public async Task<Enrollment> Add(Enrollment item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException)
            {
                throw new UnableToAddException("Unable to add Enrollment. Please check the data and try again.");
            }
        }

        public async Task<Enrollment> Delete(int key)
        {
            var enrollment = await Get(key);
            if (enrollment != null)
            {
                _context.Remove(enrollment);
                await _context.SaveChangesAsync();
                return enrollment;
            }
            throw new NoSuchEnrollmentException();
        }

        public async Task<Enrollment> Get(int key)
        {
            var enrollment = await _context.Enrollments.SingleOrDefaultAsync(e => e.EnrollmentId == key);
            if (enrollment == null)
            {
                throw new NoSuchEnrollmentException();
            }
            return enrollment;
        }

        public async Task<IEnumerable<Enrollment>> Get()
        {
            var enrollments = await _context.Enrollments.ToListAsync();
            return enrollments;
        }

        public async Task<Enrollment> Update(Enrollment item)
        {
            var enrollment = await Get(item.EnrollmentId);
            if (enrollment != null)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchEnrollmentException();
        }

    }
}
