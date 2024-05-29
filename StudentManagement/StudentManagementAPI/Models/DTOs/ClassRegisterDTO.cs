using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models.DTOs
{
    public class ClassRegisterDTO
    {
        public int CourseOfferingId { get; set; }
        public DateTime ClassDateAndTime { get; set; }
    }
}
