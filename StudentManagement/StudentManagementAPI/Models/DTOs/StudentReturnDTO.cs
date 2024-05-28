namespace StudentManagementAPI.Models.DTOs
{
    public class StudentReturnDTO
    {
        public int StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string RollNo { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
