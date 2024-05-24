using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models
{
    public class Degree
    {
        [Key]
        public int DegreeId { get; set; }

        [Required(ErrorMessage = "Degree name is required")]
        [MaxLength(255, ErrorMessage = "Degree name cannot exceed 255 characters")]
        public string DegreeName { get; set; }

        public ICollection<Branch> Branches { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
