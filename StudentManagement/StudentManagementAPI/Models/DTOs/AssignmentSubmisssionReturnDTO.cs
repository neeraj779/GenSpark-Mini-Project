using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementAPI.Models.DTOs
{
    public class AssignmentSubmisssionReturnDTO
    {
        public int AssignmentId { get; set; }
        public string FileName { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime SubmissionDate { get; set; }
    }
}
