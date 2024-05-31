namespace StudentManagementAPI.Models.DTOs
{
    public class ClassReturnDTO
    {
        public int ClassId { get; set; }
        public int CourseOfferingId { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public DateTime ClassDateAndTime { get; set; }
    }
}
