using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models.DTOs
{
    public class AssignmentUpdateDTO
    {
        public int AssignmentId { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime AssignmentDueDate { get; set; }
    }
}
