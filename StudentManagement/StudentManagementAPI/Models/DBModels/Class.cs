using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementAPI.Models.DBModels
{
    public class Class
    {
        [Key]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Course offering ID is required")]
        public int CourseOfferingId { get; set; }
        public DateTime ClassDateAndTime { get; set; }
        public CourseOffering CourseOffering { get; set; }
        public ICollection<ClassAttendance> ClassAttendances { get; set; }
    }
}
