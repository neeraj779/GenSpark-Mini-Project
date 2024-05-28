namespace StudentManagementAPI.Models.DTOs
{
    public class AssignmentDTO
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; }
        public DateTime AssignmentDueDate { get; set; }
        public string CourseCode { get; set; }
    }
}
