using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models.DBModels
{
    public class Branch
    {
        [Key]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Branch name is required")]
        [MaxLength(255, ErrorMessage = "Branch name cannot exceed 255 characters")]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "Degree ID is required")]
        public int DegreeId { get; set; }

        public Degree Degree { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<CourseOffering> CourseOfferings { get; set; }
    }
}
