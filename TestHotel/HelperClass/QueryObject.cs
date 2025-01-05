namespace BhoomiGlobalAPI.HelperClass
{
    public class QueryObject
    {
        public bool IsSortAsc { get; set; }
        public string SortBy { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool printall { get; set; }
    }

    public class QueryObjectPage : QueryObject
    {
        public int pageCategoryId { get; set; }
        public string SearchString { get; set; }
    }


    public class CarouselQueryObject : QueryObject
    {
        public int Type { get; set; }
        public string SearchString { get; set; }
    }

    public class NewsletterSubscriberQueryObject : QueryObject
    {
        public int Status { get; set; }
        public string SearchText { get; set; }
        public bool printall { get; set; }
    }

    public class NewsletterQueryObject : QueryObject
    {
        public int Status { get; set; }
        public string SearchText { get; set; }
        public bool printall { get; set; }
    }

    public class EmailQuerySearchQueryObject : QueryObject
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public int Cancelled { get; set; }
        public string SearchString { get; set; }
        public string Status { get; set; }
        public string QueuedOn { get; set; }
        public string SentOn { get; set; }
        public DateTime QueuedStartDate { get; set; }
        public DateTime QueuedEndDate { get; set; }
        public DateTime SentOnStartDate { get; set; }
        public DateTime SentOnEndDate { get; set; }
        public bool printall { get; set; }
    }
    public class EmailLogSearchQueryObject : QueryObject
    {
        public int IsSent { get; set; }
        public string SearchString { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SelectedOption { get; set; }
        public bool printall { get; set; }
    }

    public class EmailTemplateSearchQueryObject : QueryObject
    {
        public int EmailTemplateId { get; set; }
        public int Active { get; set; }
        public string SearchString { get; set; }
        public bool printall { get; set; }
    }

    public class UserDetailsQueryObject : QueryObject
    {
        public string Filtertext { get; set; }
        public long RoleId { get; set; }
        public int UserStatusId { get; set; }
        public bool printall { get; set; }
    }

}
