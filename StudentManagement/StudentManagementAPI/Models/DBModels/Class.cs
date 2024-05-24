using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models
{
    public class Class
    {
        [Key]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Course offering ID is required")]
        public int CourseOfferingId { get; set; }

        [Required(ErrorMessage = "Schedule date and time are required")]
        public DateTime Schedule { get; set; }

        public CourseOffering CourseOffering { get; set; }
        public ICollection<ClassAttendance> ClassAttendances { get; set; }
    }
}
