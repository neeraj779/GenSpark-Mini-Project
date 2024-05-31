namespace StudentManagementAPI.Models.DTOs
{
    public class UserRegisterDTO
    {
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; }
    }
}
