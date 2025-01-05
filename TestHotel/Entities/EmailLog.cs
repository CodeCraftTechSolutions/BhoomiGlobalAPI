using System.ComponentModel.DataAnnotations;

namespace BhoomiGlobalAPI.Entities
{
    public class EmailLog
    {
        [Key]
        public Int64 EmailLogId { get; set; }

        [StringLength(512)]
        public string Subject { get; set; }

        public string Body { get; set; }

        [StringLength(250)]
        public string ToEmail { get; set; }

        [StringLength(250)]
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