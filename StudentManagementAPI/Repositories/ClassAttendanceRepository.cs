using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;

namespace StudentManagementAPI.Repositories
{
    public class ClassAttendanceRepository : IRepository<int, ClassAttendance>
    {
        public readonly StudentManagementContext _context;

        public ClassAttendanceRepository(StudentManagementContext context)
        {
            _context = context;
        }

        public async Task<ClassAttendance> Add(ClassAttendance item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException)
            {
                throw new UnableToAddException("Unable to add class attendance. Please check the data and try again.");
            }
        }

        public async Task<ClassAttendance> Get(int key)
        {
            var classAttendance = await _context.ClassAttendances.SingleOrDefaultAsync(ca => ca.AttendanceId == key);
            return classAttendance;
        }

        public async Task<ClassAttendance> Delete(int key)
        {
            var classAttendance = await Get(key);
            if (classAttendance != null)
            {
                _context.ClassAttendances.Remove(classAttendance);
                await _context.SaveChangesAsync();
                return classAttendance;
            }
            throw new NoSuchClassAttendanceException();
        }


        public async Task<IEnumerable<ClassAttendance>> Get()
        {
            var classAttendances = await _context.ClassAttendances.ToListAsync();
            return classAttendances;
        }

        public async Task<ClassAttendance> Update(ClassAttendance item)
        {
            var classAttendance = await Get(item.AttendanceId);
            if (classAttendance != null)
            {
                _context.ClassAttendances.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchClassAttendanceException();
        }
    }
}
