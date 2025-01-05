namespace BhoomiGlobalAPI.DTOs
{
    public class EmailLogDTO
    {
        public Int64 EmailLogId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public bool? SendCopy { get; set; }
        public string Attachments { get; set; }
        public string BCC { get; set; }
        public string CC { get; set; }
        public Int64? SendById { get; set; }
        public DateTime? SendDate { get; set; }
        public bool? IsSent { get; set; }
        public string Error { get; set; }
    }
}
