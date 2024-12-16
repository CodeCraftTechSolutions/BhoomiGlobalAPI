namespace BhoomiGlobalAPI.DTOs
{
    public class PageCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
    }
    public class PageCategoryCode
    {
        public int CategoryCode { get; set; }
    }
}
