using System.ComponentModel.DataAnnotations;

namespace AnnouncementAPI.DTOs
{
    public class AnnouncementDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string SubCategory { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}