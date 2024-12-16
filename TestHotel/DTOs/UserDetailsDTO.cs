namespace BhoomiGlobalAPI.DTOs
{
    public class UserDetailsDTO
    {
        public long Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public String PhoneNumber { get; set; }
        public string Citizenship { get; set; }
        public DateTime RegisteredDate { get; set; }
    }
}
