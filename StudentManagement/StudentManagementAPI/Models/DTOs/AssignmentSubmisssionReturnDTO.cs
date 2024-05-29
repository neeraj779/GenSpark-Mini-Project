using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
