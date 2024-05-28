namespace StudentManagementAPI.Models.DTOs
{
    public class EnrollmentReturnDTO
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public string CourseCode { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
