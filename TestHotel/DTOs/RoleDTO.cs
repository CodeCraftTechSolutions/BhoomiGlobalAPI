namespace BhoomiGlobalAPI.DTO
{
    public class RoleDTO
    {

        public long Id { get; set; }
        public string? RoleId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int? GroupNo { get; set; }
        public int? OrderBy { get; set; }
        public bool? ShowStoreDropDown { get; set; }
        public bool? IsUniqueRole { get; set; }
    }
}
