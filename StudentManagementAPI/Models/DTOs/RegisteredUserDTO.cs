namespace StudentManagementAPI.Models.DTOs
{
    public class RegisteredUserDTO
    {
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
