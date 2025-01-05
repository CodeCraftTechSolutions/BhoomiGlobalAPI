namespace BhoomiGlobalAPI.DTOs
{
    public class UserDetailsDTO
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? DateOfBirth { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Citizenship { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public bool? IsUserLocked { get; set; }
        public int? Status { get; set; }
        public List<long>? RoleList { get; set; }
        public List<string>? RoleListStr { get; set; }
        public string? RoleNames { get; set; }
        public string? ImagePath { get; set; }
    }


    public class CheckEmailExists
    {
        public string Email { get; set; }
    }

    public class UserDetailsData
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string ImagePath { get; set; }

    }
        
}
