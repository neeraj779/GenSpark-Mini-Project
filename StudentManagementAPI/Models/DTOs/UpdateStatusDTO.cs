namespace StudentManagementAPI.Models.DTOs
{
    public class UpdateStatusDTO
    {
        public int StudentId { get; set; }
        public string Status { get; set; } = String.Empty;
    }
}
