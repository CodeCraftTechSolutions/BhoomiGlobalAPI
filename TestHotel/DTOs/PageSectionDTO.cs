namespace BhoomiGlobalAPI.DTOs
{
    public class PageSectionDTO
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int PageId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }


    public class PageSectionInputDTO
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int PageId { get; set; }
        public List<PageSectionDetailsDTO> PageSectionDetailsList { get; set; }
    }

    public class PageSectionUpsertDTO
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int PageId { get; set; }
        public List<PageSectionInputDTO> PageSectionInputList { get; set; }
    }


}
