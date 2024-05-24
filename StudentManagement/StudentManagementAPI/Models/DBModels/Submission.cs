using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models
{
    public class Submission
    {
        [Key]
        public int SubmissionId { get; set; }

        [Required(ErrorMessage = "Assignment ID is required.")]
        public int AssignmentId { get; set; }

        [Required(ErrorMessage = "Student ID is required.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Submission date is required.")]
        [DataType(DataType.Date)]
        public DateTime SubmissionDate { get; set; }

        [Required(ErrorMessage = "Completion status is required.")]
        public bool IsCompleted { get; set; }

        public Assignment Assignment { get; set; }
        public Student Student { get; set; }
    }
}
