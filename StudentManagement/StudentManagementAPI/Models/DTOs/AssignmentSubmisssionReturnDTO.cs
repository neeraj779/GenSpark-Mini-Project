using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models.DTOs
{
    public class AssignmentSubmisssionReturnDTO
    {
        public string AssignmentId { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime SubmissionDate { get; set; }
        public string stautus { get; set; }

    }
}
