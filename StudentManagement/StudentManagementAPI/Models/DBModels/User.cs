using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace StudentManagementAPI.Models.DBModels
{
    public enum UserRole
    {
        Admin,
        Teacher,
        Student
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "User name must be between 3 and 50 characters.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public byte[] Password { get; set; } = Array.Empty<byte>();

        [Required(ErrorMessage = "Password hash key is required.")]
        public byte[] PasswordHashKey { get; set; } = Array.Empty<byte>();

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status must be between 1 and 20 characters.")]
        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(20, ErrorMessage = "Role must be between 1 and 20 characters.")]
        public UserRole Role { get; set; }

        [Required(ErrorMessage = "Registration date is required.")]
        [Column(TypeName = "datetime")]
        public DateTime RegistrationDate { get; set; }
    }
}
