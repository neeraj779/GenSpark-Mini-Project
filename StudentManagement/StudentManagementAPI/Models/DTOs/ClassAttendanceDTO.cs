namespace StudentManagementAPI.Models.DTOs
{
    public class ClassAttendanceDTO
    {
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public string Status { get; set; }
    }
}
