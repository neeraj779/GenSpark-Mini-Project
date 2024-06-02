using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementAPI.Models.DBModels
{
    public class CourseOffering
    {
        [Key]
        public int CourseOfferingId { get; set; }

        [Required(ErrorMessage = "Course Code is required")]
        public string CourseCode { get; set; } = string.Empty;
        public int TeacherId { get; set; }

        [ForeignKey("CourseCode")]
        public Course? Course { get; set; }
        public Teacher? Teacher { get; set; }
        public ICollection<Class>? Classes { get; set; }
    }
}
