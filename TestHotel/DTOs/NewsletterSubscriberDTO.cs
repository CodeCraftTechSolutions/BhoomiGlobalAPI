namespace BhoomiGlobalAPI.DTOs
{
    public class NewsletterSubscriberDTO
    {
        public Int64 Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? IPAddress { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Int64? CreatedById { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Int64? ModifiedById { get; set; }
    }
}
