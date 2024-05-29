using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models.DTOs
{
    public class StudentRegisterDTO
    {
        public string FullName { get; set; }
        public string RollNo { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;

        [MaxLength(10, ErrorMessage = "Phone number cannot exceed 10 characters")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Degree { get; set; }
        public string Branch { get; set; }
    }
}
