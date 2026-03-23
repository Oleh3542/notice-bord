namespace AnnouncementWEB.Models
{
    public class AnnouncementViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}