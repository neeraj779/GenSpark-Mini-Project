using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IRepository<int, Assignment> _assignmentRepository;
        private readonly IRepository<string, Course> _courseRepository;

        public AssignmentService(IRepository<int, Assignment> assignmentRepository, IRepository<string, Course> courseRepository)
        {
            _assignmentRepository = assignmentRepository;
            _courseRepository = courseRepository;
        }

        public async Task<AssignmentDTO> CreateAssignment(CreateAssignmentDTO assignment)
        {
            await EnsureCourseExists(assignment.CourseCode);
            await EnsureAssignmentDoesNotExist(assignment.CourseCode, assignment.Title);

            var newAssignment = new Assignment
            {
                Title = assignment.Title,
                DueDate = assignment.AssignmentDueDate,
                CourseCode = assignment.CourseCode
            };

            var createdAssignment = await _assignmentRepository.Add(newAssignment);
            return MapToDTO(createdAssignment);
        }

        public async Task<AssignmentDTO> DeleteAssignment(int assignmentId)
        {
            var assignment = await EnsureAssignmentExists(assignmentId);
            await _assignmentRepository.Delete(assignment.AssignmentId);
            return MapToDTO(assignment);
        }

        public async Task<AssignmentDTO> GetAssignmentById(int assignmentId)
        {
            var assignment = await EnsureAssignmentExists(assignmentId);
            return MapToDTO(assignment);
        }

        public async Task<IEnumerable<AssignmentDTO>> GetAssignments()
        {
            var assignments = await _assignmentRepository.Get();

            if (!assignments.Any())
                throw new NoAssignmentFoundException();

            return assignments.Select(MapToDTO).ToList();
        }

        public async Task<AssignmentDTO> UpdateAssignmentDueDate(AssignmentUpdateDTO assignmentUpdateDto)
        {
            var assignment = await EnsureAssignmentExists(assignmentUpdateDto.AssignmentId);
            assignment.DueDate = assignmentUpdateDto.AssignmentDueDate;

            var updatedAssignment = await _assignmentRepository.Update(assignment);
            return MapToDTO(updatedAssignment);
        }

        private async Task<Course> EnsureCourseExists(string courseCode)
        {
            var course = await _courseRepository.Get(courseCode);
            if (course == null)
                throw new NoSuchCourseException();
            return course;
        }

        private async Task<bool> EnsureAssignmentDoesNotExist(string courseCode, string title)
        {
            var isAssignmentExists = await _assignmentRepository.Get();
            if (isAssignmentExists.Any(a => a.Title == title && a.CourseCode == courseCode))
                throw new AssignmentAlreadyExistsException();

            return true;
        }

        private async Task<Assignment> EnsureAssignmentExists(int assignmentId)
        {
            var assignment = await _assignmentRepository.Get(assignmentId);
            if (assignment == null)
                throw new NoSuchAssignmentException();
            return assignment;
        }

        private AssignmentDTO MapToDTO(Assignment assignment)
        {
            return new AssignmentDTO
            {
                AssignmentId = assignment.AssignmentId,
                Title = assignment.Title,
                AssignmentDueDate = assignment.DueDate,
                CourseCode = assignment.CourseCode
            };
        }
    }
}
