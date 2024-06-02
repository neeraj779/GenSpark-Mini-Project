using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementAPI.Models.DBModels
{
    public enum AttendanceStatus
    {
        Present,
        Absent,
        Late,
        Excused
    }
    public class ClassAttendance
    {
        [Key]
        public int AttendanceId { get; set; }

        [Required(ErrorMessage = "Class ID is required")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Student ID is required")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        [MaxLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public AttendanceStatus Status { get; set; }

        public Class? Class { get; set; }
        public Student? Student { get; set; }
    }
}
