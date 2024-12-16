using BhoomiGlobalAPI.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhoomiGlobalAPI.Entities
{
    public class Role
    {
        public long Id { get; set; }
        public string? RoleId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string DisplayName { get; set; }
        public int GroupNo { get; set; }
        public int? OrderBy { get; set; }
        public bool? ShowStoreDropDown { get; set; }
        public bool? IsUniqueRole { get; set; }
        public bool? IsVisible { get; set; } = false;
    }

    public class UserRole
    {
        public long Id { get; set; }

        [Required]
        [ForeignKey("UserDetail")]
        public long UserId { get; set; }
        public virtual UserDetails UserDetail { get; set; }

        [Required]
        [ForeignKey("Role")]
        public long RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
