using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;

namespace StudentManagementAPI.Repositories
{
    public class AssignmentSubmissionRepository : IRepository<int, Submission>
    {
        public readonly StudentManagementContext _context;

        public AssignmentSubmissionRepository(StudentManagementContext context)
        {
            _context = context;
        }

        public async Task<Submission> Add(Submission item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException)
            {
                throw new UnableToAddException("Unable to add assignment submission. Please check the data and try again.");
            }
        }

        public async Task<Submission> Delete(int key)
        {
            var submission = await Get(key);
            if (submission != null)
            {
                _context.Remove(submission);
                await _context.SaveChangesAsync();
                return submission;
            }
            throw new NoSuchSubmissionException();
        }

        public async Task<Submission> Get(int key)
        {
            var submission = await _context.Submissions.SingleOrDefaultAsync(s => s.SubmissionId == key);
            return submission;
        }

        public async Task<IEnumerable<Submission>> Get()
        {
            var submissions = await _context.Submissions.ToListAsync();
            return submissions;
        }

        public async Task<Submission> Update(Submission item)
        {
            var submission = await Get(item.SubmissionId);
            if (submission != null)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchSubmissionException();
        }
    }
}
