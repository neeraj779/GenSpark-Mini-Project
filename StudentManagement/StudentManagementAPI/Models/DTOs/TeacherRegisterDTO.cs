namespace StudentManagementAPI.Models.DTOs
{
    public class TeacherRegisterDTO
    {
        public string FullName { get; set; }
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; }
    }
}
