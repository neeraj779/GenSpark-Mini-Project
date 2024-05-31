using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models.DTOs
{
    public class AssignmentSubmisssionDTO
    {
        [Required]
        public int AssignmentId { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
