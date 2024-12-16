namespace BhoomiGlobalAPI.DTOs
{
    public class PageDTO
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string? BannerImgUrl { get; set; } 
        public string? SmallBannerImgUrl { get; set; }
        public int PageCategoryId { get; set; }
        public string PageName { get; set; }
        public string PageUrlCode { get; set; }

        public string PageCategoryName { get; set; }
        public virtual PageCategoryDTO PageCategory { get; set; }
        public virtual ICollection<PageImageDTO> PageImages { get; set; }
        public PageSectionUpsertDTO? SectionsDetailsList { get; set; }

    }
    public class PageImageDTO
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public string ImagePath { get; set; }
        public bool IsPrimary { get; set; }
        public ICollection<PageImageDTO> PageImages { get; set; }
    }
    public class PageImageCheckPrimaryDTO
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public bool IsPrimary { get; set; }

    }

    public class PageViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ImageUrl { get; set; }

    }
    public class PageBundle
    {
        public int PageCategoryId { get; set; }
        public string PageCategoryName { get; set; }
        public string Description { get; set; }
        public IEnumerable<PageViewModel> PageModifiedDTOs { get; set; }

    }
}
