namespace StudentManagementAPI.Models.DTOs
{
    public class UpdateStatusDTO
    {
        public int studentId { get; set; }
        public string Status { get; set; } = String.Empty;
    }
}
