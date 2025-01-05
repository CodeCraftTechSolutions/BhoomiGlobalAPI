using System.ComponentModel.DataAnnotations;

namespace BhoomiGlobalAPI.Entities
{
    public class EmailQueue
    {
        [Key]
        public Int64 Id { get; set; }
        public string TemplateName { get; set; }
        public int PriorityId { get; set; }

        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string ReplyToEmail { get; set; }
        public string ReplyToName { get; set; }
        public string CcEmail { get; set; }
        public string BccEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string CallBackUrl { get; set; }
        public string TempPassword { get; set; }
        public int StatusId { get; set; }
        public int FailCount { get; set; }
        public String Attachements { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime QueuedOn { get; set; }
        public DateTime? SentOn { get; set; }
        public Int64 UserId { get; set; }
        public bool Cancelled { get; set; }
        public Boolean AddNotification { get; set; }
        public string Data { get; set; }
        public DateTime? CancelledOn { get; set; }
        public long? OrderId { get; set; }
        public int? BookingTypeEnum { get; set; } = 0;
        public bool? IsAdmin { get; set; } = false;
        public string? Password { get; set; }
    }
}
