using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BhoomiGlobalAPI.DTOs
{
    public class NewsletterDTO
    {
        public Int64 Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Subject { get; set; }

        public string EmailContent { get; set; }
        public int Status { get; set; }
        public string statusString { get; set; }
        [Required]
        public DateTime SendOn { get; set; }
        public Int64 ModifiedById { get; set; }
    }

    public class NewsletterSubscriberAPIDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
    public class NewsletterAPIDTO
    {
        public Int64 Id { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public List<NewsletterSubscriberAPIDTO> Subscribers { get; set; }
        public List<KeyNamePair> KeyNamePairs { get; set; }
    }
}
