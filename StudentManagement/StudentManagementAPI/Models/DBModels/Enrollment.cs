using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementAPI.Models.DBModels
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [Required(ErrorMessage = "Student ID is required.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Course ID is required.")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Enrollment date is required.")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
    }
}