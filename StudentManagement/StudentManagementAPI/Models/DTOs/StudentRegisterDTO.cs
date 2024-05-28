namespace StudentManagementAPI.Models.DTOs
{
    public class StudentRegisterDTO
    {
        public string FullName { get; set; }
        public string RollNo { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Degree { get; set; }
        public string Branch { get; set; }
    }
}
