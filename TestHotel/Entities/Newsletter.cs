namespace BhoomiGlobalAPI.Entities
{
    public class Newsletter
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string EmailContent { get; set; }
        public int Status { get; set; }
        public DateTime SendOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int64 CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int64 ModifiedById { get; set; }
    }
}
