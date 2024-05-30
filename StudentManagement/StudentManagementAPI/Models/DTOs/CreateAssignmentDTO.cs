using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementAPI.Models.DTOs
{
    public class CreateAssignmentDTO
    {
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime AssignmentDueDate { get; set; }
        public string CourseCode { get; set; }
    }
}
