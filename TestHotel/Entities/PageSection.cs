using System.ComponentModel.DataAnnotations.Schema;

namespace BhoomiGlobalAPI.Entities
{
    public class PageSection
    {
        public long Id { get; set; }    
        public string? Title { get; set; }
        public int PageId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public virtual List<PageSectionDetails> PageSectionDetailsList { get; set; }
        [ForeignKey("PageId")]
        public virtual Page Page {  get; set; }
    }
}
