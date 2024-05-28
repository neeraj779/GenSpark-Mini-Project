using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models.DTOs
{
    public class AssignmentDTO
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime AssignmentDueDate { get; set; }
        public string CourseCode { get; set; }
    }
}
