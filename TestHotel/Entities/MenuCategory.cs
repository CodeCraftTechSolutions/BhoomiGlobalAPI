using System.ComponentModel.DataAnnotations;


namespace BhoomiGlobalAPI.Entities
{
    public class MenuCategory
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public Int64 CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int64 ModifiedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
