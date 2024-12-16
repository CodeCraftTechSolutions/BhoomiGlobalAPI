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


}
