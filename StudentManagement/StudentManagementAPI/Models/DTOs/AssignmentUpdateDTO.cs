using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
