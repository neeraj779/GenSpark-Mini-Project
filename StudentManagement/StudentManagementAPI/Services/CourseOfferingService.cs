using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Services
{
    public class CourseOfferingService : ICourseOfferingService
    {
        private IRepository<int, CourseOffering> _courseOfferingRepository;
        private IRepository<string, Course> _courseRepository;
        private IRepository<int, Teacher> _teacherRepository;

        public CourseOfferingService(
            IRepository<int, CourseOffering> courseOfferingRepository,
            IRepository<string, Course> courseRepository,
            IRepository<int, Teacher> teacherRepository
            )
        {
            _courseOfferingRepository = courseOfferingRepository;
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
        }
        public async Task<CourseOfferingDTO> AssignTeacherForCourseOffering(int teacherid, string CourseCode)
        {
            var course = await _courseRepository.Get(CourseCode);
            var teacher = await _teacherRepository.Get(teacherid);

            if (course == null)
                throw new NoSuchCourseException();

            if (teacher == null)
                throw new NoSuchTeacherException();

            var isCourseOfferingExists = await _courseOfferingRepository.Get();
            var courseOfferingExists = isCourseOfferingExists.FirstOrDefault(courseOffering => courseOffering.CourseCode == CourseCode && courseOffering.TeacherId == teacherid);

            if (courseOfferingExists != null)
                throw new CourseOfferingAlreadyExistsException();

            var courseOffering = new CourseOffering();
            courseOffering.CourseCode = course.CourseCode;
            courseOffering.TeacherId = teacher.TeacherId;

            var createdCourseOffering = await _courseOfferingRepository.Add(courseOffering);

            return MapCourseOfferingToCourseOfferingDTO(createdCourseOffering);
        }

        public async Task<IEnumerable<CourseOfferingDTO>> GetAllCourseOfferings()
        {
            var courseOfferings = await _courseOfferingRepository.Get();

            if (courseOfferings.Count() == 0)
                throw new NoCourseOfferingException();

            var courseOfferingDTOs = new List<CourseOfferingDTO>();

            foreach (var courseOffering in courseOfferings)
            {
                courseOfferingDTOs.Add(MapCourseOfferingToCourseOfferingDTO(courseOffering));
            }
            return courseOfferingDTOs;
        }

        public async Task<IEnumerable<CourseOfferingDTO>> GetcourseOfferingByCourseCode(string courseCode)
        {
            var course = await _courseRepository.Get(courseCode);

            if (course == null)
                throw new NoSuchCourseException();

            var courseOfferings = await _courseOfferingRepository.Get();

            var courseOfferingsByCourseCode = courseOfferings.Where(courseOffering => courseOffering.CourseCode == courseCode).ToList();

            if (courseOfferingsByCourseCode.Count == 0)
                throw new NoCourseOfferingException();

            var courseOfferingDTOs = new List<CourseOfferingDTO>();

            foreach (var courseOffering in courseOfferingsByCourseCode)
                courseOfferingDTOs.Add(MapCourseOfferingToCourseOfferingDTO(courseOffering));

            return courseOfferingDTOs;
        }

        public async Task<IEnumerable<CourseOfferingDTO>> GetcourseOfferingByTeacherId(int teacherId)
        {
            var teacher = await _teacherRepository.Get(teacherId);

            if (teacher == null)
                throw new NoSuchTeacherException();

            var courseOfferings = await _courseOfferingRepository.Get();

            var courseOfferingByTeacherId = courseOfferings.Where(courseOffering => courseOffering.TeacherId == teacherId);

            if (courseOfferingByTeacherId.Count() == 0)
                throw new NoCourseOfferingException();

            var courseOfferingDTOs = new List<CourseOfferingDTO>();

            foreach (var courseOffering in courseOfferingByTeacherId)
                courseOfferingDTOs.Add(MapCourseOfferingToCourseOfferingDTO(courseOffering));

            return courseOfferingDTOs;
        }

        public async Task<CourseOfferingDTO> UnassignTeacherFromCourseOffering(int teacherid, string CourseCode)
        {
            var course = await _courseRepository.Get(CourseCode);
            var teacher = await _teacherRepository.Get(teacherid);

            if (course == null)
                throw new NoSuchCourseException();

            if (teacher == null)
                throw new NoSuchTeacherException();

            var courseOffering = await _courseOfferingRepository.Get();
            var courseOfferingToUnassign = courseOffering.FirstOrDefault(courseOffering => courseOffering.CourseCode == CourseCode && courseOffering.TeacherId == teacherid);

            if (courseOfferingToUnassign == null)
                throw new NoSuchCourseOfferingException();

            await _courseOfferingRepository.Delete(courseOfferingToUnassign.CourseOfferingId);

            return MapCourseOfferingToCourseOfferingDTO(courseOfferingToUnassign);
        }

        public async Task<CourseOfferingDTO> UpdateTeacherForCourseOffering(int teacherid, int courseOfferingId)
        {
            var teacher = await _teacherRepository.Get(teacherid);
            var courseoffering = await _courseOfferingRepository.Get(courseOfferingId);

            if (teacher == null)
                throw new NoSuchTeacherException();

            if (courseoffering == null)
                throw new NoSuchCourseOfferingException();

            courseoffering.TeacherId = teacher.TeacherId;
            await _courseOfferingRepository.Update(courseoffering);

            return MapCourseOfferingToCourseOfferingDTO(courseoffering);
        }

        public CourseOfferingDTO MapCourseOfferingToCourseOfferingDTO(CourseOffering courseOffering)
        {
            return new CourseOfferingDTO
            {
                CourseOfferingId = courseOffering.CourseOfferingId,
                CourseCode = courseOffering.CourseCode,
                TeacherId = courseOffering.TeacherId
            };
        }
    }
}
