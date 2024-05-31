namespace StudentManagementAPI.Models.DTOs
{
    public class ClassAttendanceReturnDTO
    {
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}
