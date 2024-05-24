using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        public int? UserId { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Roll number is required.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Roll number must contain only letters and numbers.")]
        public string RollNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number.")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Degree is required.")]
        [StringLength(50, ErrorMessage = "Degree name cannot exceed 50 characters.")]
        public int DegreeId { get; set; }

        [Required(ErrorMessage = "BranchId is required.")]
        [StringLength(150, ErrorMessage = "BranchId name cannot exceed 150 characters.")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status must be between 1 and 20 characters.")]
        public string Status { get; set; } = string.Empty;

        public User User { get; set; }
        public Branch Branch { get; set; }
        public Degree Degree { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<ClassAttendance> ClassAttendances { get; set; }
        public ICollection<Submission> Submissions { get; set; }
    }
}
