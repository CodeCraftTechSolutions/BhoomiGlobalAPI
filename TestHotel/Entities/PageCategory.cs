using System.ComponentModel.DataAnnotations;


namespace BhoomiGlobalAPI.Entities
{
    public class PageCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        public int CategoryCode { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
        public ICollection<Page> Pages { get; set; }
    }
}
