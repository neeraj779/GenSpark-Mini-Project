using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models
{
    public class ClassAttendance
    {
        [Key]
        public int AttendanceId { get; set; }

        [Required(ErrorMessage = "Class ID is required")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Student ID is required")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [MaxLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string Status { get; set; }

        public Class Class { get; set; }
        public Student Student { get; set; }
    }
}
