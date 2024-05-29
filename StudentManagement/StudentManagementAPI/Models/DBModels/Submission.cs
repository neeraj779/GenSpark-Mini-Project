using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementAPI.Models.DBModels
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
        [Column(TypeName = "Date")]
        public DateTime SubmissionDate { get; set; }

        public string FileName { get; set; }
        public Assignment Assignment { get; set; }
        public Student Student { get; set; }
    }
}
