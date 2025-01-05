namespace BhoomiGlobalAPI.DTOs
{
    public class ContactDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string InquiryType { get; set; }
        public string InquiryMessage { get; set; }
        public DateTime? InquiryDate { get; set; }
    }
}
