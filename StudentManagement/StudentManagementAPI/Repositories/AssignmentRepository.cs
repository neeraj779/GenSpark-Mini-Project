using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;

namespace StudentManagementAPI.Exceptions
{
    public class AssignmentRepository : IRepository<int, Assignment>
    {
        private readonly StudentManagementContext _context;

        public AssignmentRepository(StudentManagementContext context)
        {
            _context = context;
        }

        public async Task<Assignment> Add(Assignment item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException)
            {
                throw new UnableToAddException("Unable to add assignment. Please check the data and try again.");
            }
        }

        public async Task<Assignment> Get(int key)
        {
            var assignment = await _context.Assignments.SingleOrDefaultAsync(a => a.AssignmentId == key);
            return assignment;
        }

        public async Task<IEnumerable<Assignment>> Get()
        {
            var assignments = await _context.Assignments.ToListAsync();
            return assignments;
        }

        public async Task<Assignment> Update(Assignment item)
        {
            var assignment = await Get(item.AssignmentId);
            if (assignment != null)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }

            throw new NoSuchAssignmentException();
        }

        public async Task<Assignment> Delete(int key)
        {
            var assignment = await Get(key);
            if (assignment != null)
            {
                _context.Remove(assignment);
                await _context.SaveChangesAsync();
                return assignment;
            }
            throw new NoSuchAssignmentException();
        }
    }
}
