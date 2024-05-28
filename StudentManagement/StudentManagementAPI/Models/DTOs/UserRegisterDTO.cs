namespace StudentManagementAPI.Models.DTOs
{
    public class UserRegisterDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; }
    }
}
