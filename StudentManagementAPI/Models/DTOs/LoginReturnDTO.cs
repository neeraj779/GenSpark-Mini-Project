namespace StudentManagementAPI.Models.DTOs
{
    public class LoginReturnDTO
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string Role { get; set; }
    }
}
