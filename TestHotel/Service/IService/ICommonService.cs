namespace BhoomiGlobalAPI.Service.IService
{
    public interface ICommonService
    {
        Tuple<DateTime?, DateTime?> getDateRange(string DatedOn, DateTime? Fromval, DateTime? Tillval);
    }
}
