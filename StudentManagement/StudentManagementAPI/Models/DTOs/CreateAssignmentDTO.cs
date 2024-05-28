namespace StudentManagementAPI.Models.DTOs
{
    public class CreateAssignmentDTO
    {
        public string Title { get; set; }
        public DateTime AssignmentDueDate { get; set; }
        public string CourseCode { get; set; }
    }
}
