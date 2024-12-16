namespace BhoomiGlobalAPI.Entities
{
    public class UserDetails
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public String? PhoneNumber { get; set; }
        public DateTime? RegisteredDate { get; set; }
    }


}
