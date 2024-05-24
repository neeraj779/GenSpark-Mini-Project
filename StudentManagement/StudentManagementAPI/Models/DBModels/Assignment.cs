using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models
{
    public class Assignment
    {
        [Key]
        public int AssignmentId { get; set; }

        [Required(ErrorMessage = "Course ID is required.")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public Course Course { get; set; }
        public ICollection<Submission> Submissions { get; set; }
    }
}
