using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Services
{
    public class AssignmentService : IAssignmentService
    {
        private IRepository<int, Assignment> _assignmentRepository;

        public AssignmentService(IRepository<int, Assignment> assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }
        public async Task<AssignmentDTO> CreateAssignment(CreateAssignmentDTO assignment)
        {
            var newAssignment = new Assignment();

            newAssignment.Title = assignment.Title;
            newAssignment.DueDate = assignment.AssignmentDueDate;
            newAssignment.CourseCode = assignment.CourseCode;

            var createdAssignment = await _assignmentRepository.Add(newAssignment);

            return MapAssignmentToAssignmentDTO(createdAssignment);
        }

        public async Task<AssignmentDTO> DeleteAssignment(int assignmentId)
        {
            var deletedAssignment = await _assignmentRepository.Delete(assignmentId);
            return MapAssignmentToAssignmentDTO(deletedAssignment);
        }

        public async Task<AssignmentDTO> GetAssignmentById(int assignmentId)
        {
            var assignment =await _assignmentRepository.Get(assignmentId);
            return MapAssignmentToAssignmentDTO(assignment);
        }

        public async Task<IEnumerable<AssignmentDTO>> GetAssignments()
        {
            var assignments = await _assignmentRepository.Get();
            var assignmentDTOs = new List<AssignmentDTO>();
            foreach (var assignment in assignments)
                assignmentDTOs.Add(MapAssignmentToAssignmentDTO(assignment));

            return assignmentDTOs;

        }

        public async Task<AssignmentDTO> UpdateAssignmentDueDate(int assignmentId, DateTime dueDate)
        {
            var assignment = await _assignmentRepository.Get(assignmentId);
            assignment.DueDate = dueDate;

            var updatedAssignment = await _assignmentRepository.Update(assignment);
            return MapAssignmentToAssignmentDTO(updatedAssignment);
        }

        public AssignmentDTO MapAssignmentToAssignmentDTO(Assignment assignment)
        {
            AssignmentDTO assignmentDTO = new AssignmentDTO();
            assignmentDTO.AssignmentId = assignment.AssignmentId;
            assignmentDTO.Title = assignment.Title;
            assignmentDTO.AssignmentDueDate = assignment.DueDate;
            assignmentDTO.CourseCode = assignment.CourseCode;
            return assignmentDTO;
        }
    }
}
