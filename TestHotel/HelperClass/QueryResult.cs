﻿namespace BhoomiGlobalAPI.HelperClass
{
    public class QueryResult<T>
    {
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }

    }
}
