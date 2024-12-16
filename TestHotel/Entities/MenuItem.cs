using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhoomiGlobalAPI.Entities
{
    public class MenuItem
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [ForeignKey("MenuCategoryId")]
        public int MenuCategoryId { get; set; }
        public int Status { get; set; }
        public int MenuTypeId { get; set; }
        public int EntityCategoryId { get; set; }
        public string Url { get; set; }
        public string CodeUrl { get; set; }
        public Int64 CreatedById { get; set; }
        public int EntityId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int64 ModifiedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public virtual MenuCategory MenuCategories { get; set; }


    }
}
