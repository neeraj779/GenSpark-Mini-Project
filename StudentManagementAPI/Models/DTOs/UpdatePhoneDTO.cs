using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models.DTOs
{
    public class UpdatePhoneDTO
    {
        public int Id { get; set; }

        [MaxLength(10, ErrorMessage = "Phone number cannot exceed 10 characters")]
        [MinLength(10, ErrorMessage = "Phone number must be at least 10 characters")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; } = string.Empty;
    }
}
