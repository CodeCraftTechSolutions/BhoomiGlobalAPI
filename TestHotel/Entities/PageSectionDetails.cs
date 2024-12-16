using System.ComponentModel.DataAnnotations.Schema;

namespace BhoomiGlobalAPI.Entities
{
    public class PageSectionDetails
    {
        public long Id { get; set; }
        public long PageSectionId { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public string? ImageUrl { get; set; }
        public string? IconUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("PageSectionId")]
        public virtual PageSection PageSection { get; set; }
    }
}
