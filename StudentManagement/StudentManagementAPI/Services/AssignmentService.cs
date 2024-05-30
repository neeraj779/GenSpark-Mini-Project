using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Services
{
    public class AssignmentService : IAssignmentService
    {
        private IRepository<int, Assignment> _assignmentRepository;
        private IRepository<string, Course> _courseRepository;

        public AssignmentService(IRepository<int, Assignment> assignmentRepository, IRepository<string, Course> courseRepository)
        {
            _assignmentRepository = assignmentRepository;
            _courseRepository = courseRepository;
        }
        public async Task<AssignmentDTO> CreateAssignment(CreateAssignmentDTO assignment)
        {
            var newAssignment = new Assignment();

            var course = await _courseRepository.Get(assignment.CourseCode);
            if (course == null)
                throw new NoSuchCourseException();

            newAssignment.Title = assignment.Title;
            newAssignment.DueDate = assignment.AssignmentDueDate;
            newAssignment.CourseCode = assignment.CourseCode;

            var createdAssignment = await _assignmentRepository.Add(newAssignment);

            return MapAssignmentToAssignmentDTO(createdAssignment);
        }

        public async Task<AssignmentDTO> DeleteAssignment(int assignmentId)
        {
            try
            {
                var deletedAssignment = await _assignmentRepository.Delete(assignmentId);
                return MapAssignmentToAssignmentDTO(deletedAssignment);
            }
            catch (NoSuchAssignmentException)
            {
                throw new NoSuchAssignmentException();
            }
        }

        public async Task<AssignmentDTO> GetAssignmentById(int assignmentId)
        {
            var assignment =await _assignmentRepository.Get(assignmentId);

            if (assignment == null)
                throw new NoSuchAssignmentException();

            return MapAssignmentToAssignmentDTO(assignment);
        }

        public async Task<IEnumerable<AssignmentDTO>> GetAssignments()
        {
            var assignments = await _assignmentRepository.Get();

            if (assignments.Count() == 0)
                throw new NoAssignmentFoundException();

            var assignmentDTOs = new List<AssignmentDTO>();
            foreach (var assignment in assignments)
                assignmentDTOs.Add(MapAssignmentToAssignmentDTO(assignment));

            return assignmentDTOs;

        }

        public async Task<AssignmentDTO> UpdateAssignmentDueDate(AssignmentUpdateDTO assignment)
        {
            var assignmentDb = await _assignmentRepository.Get(assignment.AssignmentId);

            if (assignmentDb == null)
                throw new NoSuchAssignmentException();

            assignmentDb.DueDate = assignment.AssignmentDueDate;

            var updatedAssignment = await _assignmentRepository.Update(assignmentDb);
            return MapAssignmentToAssignmentDTO(updatedAssignment);
        }

        public AssignmentDTO MapAssignmentToAssignmentDTO(Assignment assignment)
        {
            AssignmentDTO assignmentDTO = new AssignmentDTO
            {
                AssignmentId = assignment.AssignmentId,
                Title = assignment.Title,
                AssignmentDueDate = assignment.DueDate,
                CourseCode = assignment.CourseCode
            };
            return assignmentDTO;
        }
    }
}
