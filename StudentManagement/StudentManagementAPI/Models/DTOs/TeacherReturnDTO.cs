using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models.DTOs
{
    public class TeacherReturnDTO
    {
        public int TeacherId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; }
    }
}
