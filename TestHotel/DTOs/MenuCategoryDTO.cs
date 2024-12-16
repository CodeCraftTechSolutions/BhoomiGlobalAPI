namespace BhoomiGlobalAPI.DTOs
{
    public class MenuCategoryDTO
    {
       
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string IsActive { get; set; }
        public int? ParentId { get; set; }
        public List<MenuCategoryDTO> ChildrenmenuCategory { get; set; }
        public Int64 CreatedById { get; set; }
        public String CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Icon { get; set; }
        public int ModifiedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public virtual UserDetailsDTO UserDetails { get; set; }
       
    }
}
