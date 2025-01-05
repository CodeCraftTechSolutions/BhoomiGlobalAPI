using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BhoomiGlobalAPI.Entities
{
    public class Page
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string? BannerImgUrl { get; set; } = string.Empty;
        public string? SmallBannerImgUrl { get; set; } = string.Empty;
        [ForeignKey("PageCategoryId")]
        public int? PageCategoryId { get; set; }
        public string? PageName { get; set; }
        public string? PageUrlCode { get; set; }
        public virtual PageCategory? PageCategory { get;set ;}
        public ICollection<PageImage>? PageImages { get; set; }
        public ICollection<PageSection>? PageSections { get; set; }
    }


    public class PageImage
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("PageId")]
        public int? PageId { get; set; }
        public string? ImagePath { get; set; }
        public bool? IsPrimary { get; set; }
        public virtual Page? Page { get; set; }

    }
}
