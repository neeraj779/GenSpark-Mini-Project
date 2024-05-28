using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models.DBModels
{
    public class CourseOffering
    {
        [Key]
        public int CourseOfferingId { get; set; }

        [Required(ErrorMessage = "Course ID is required")]
        public int CourseId { get; set; }
        public int? TeacherId { get; set; }

        public Course Course { get; set; }
        public Teacher Teacher { get; set; }
        public ICollection<Class> Classes { get; set; }
    }
}
