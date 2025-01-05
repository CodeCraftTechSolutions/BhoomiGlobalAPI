using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Service.IService;

namespace BhoomiGlobalAPI.Service
{
    public class CommonService: ICommonService
    {
        
        public Tuple<DateTime?,DateTime?>  getDateRange(string DatedOn, DateTime? Fromval, DateTime? Tillval)
        {
            #region DateRanges

            DateTime?  From = Fromval;
            DateTime?  Till = Tillval;
            switch (Convert.ToInt32(DatedOn))
            {
                case (int)Enums.DatingFilter.anytime:
                    From = null;
                    Till = null;
                    break;
                case (int)Enums.DatingFilter.tomorrow:
                    From = DateTime.Now.Date.AddDays(1);
                    Till = DateTime.Now.Date.AddDays(1);
                    break;
                case (int)Enums.DatingFilter.today:
                    From = DateTime.Now.Date;
                    Till = DateTime.Now.Date;
                    break;
                case (int)Enums.DatingFilter.yesterday:
                    From = DateTime.Now.Date.AddDays(-1);
                    Till = DateTime.Now.Date.AddDays(-1);
                    break;
                case (int)Enums.DatingFilter.onOrAfter:
                    Till = null;
                    break;
                case (int)Enums.DatingFilter.on:
                    Till = From;
                    break;
                case (int)Enums.DatingFilter.OnOrBefore:
                    //Till = Till;
                    From = null;
                    break;
                case (int)Enums.DatingFilter.between:
                    //From = From;
                    //Till = Till;
                    break;
                case (int)Enums.DatingFilter.nextWeek:
                    From = DateTime.Now.Date.AddDays(7).AddDays(-Convert.ToInt16(DateTime.Now.DayOfWeek));
                    Till = From.Value.AddDays(6);
                    break;
                case (int)Enums.DatingFilter.thisWeek:
                    //This week       
                    From = DateTime.Now.Date.AddDays(-Convert.ToInt16(DateTime.Now.DayOfWeek));
                    Till = From.Value.AddDays(6);
                    break;
                case (int)Enums.DatingFilter.lastWeek:
                    //Previous week       
                    From = DateTime.Now.Date.AddDays(-7).AddDays(-Convert.ToInt16(DateTime.Now.DayOfWeek));
                    Till = From.Value.AddDays(6);
                    break;
                case (int)Enums.DatingFilter.nextMonth:
                    From = DateTime.Now.Date.AddMonths(1).AddDays(-DateTime.Now.Day + 1);
                    Till = From.Value.AddMonths(1).AddDays(-1);
                    break;
                case (int)Enums.DatingFilter.thisMonth:
                    From = DateTime.Now.Date.AddDays(-DateTime.Now.Day + 1);
                    Till = From.Value.Date.AddMonths(1).AddDays(-1);
                    break;
                case (int)Enums.DatingFilter.lastMonth:
                    //Previous Month
                    From = DateTime.Now.Date.AddMonths(-1).AddDays(-DateTime.Now.Day + 1);
                    Till = From.Value.AddMonths(1).AddDays(-1);
                    break;
                case (int)Enums.DatingFilter.nextYear:
                    //Next Year
                    From = DateTime.Now.Date.AddYears(1).AddDays(-DateTime.Now.DayOfYear + 1);
                    Till = From.Value.AddYears(1).AddDays(-1);
                    break;
                case (int)Enums.DatingFilter.thisYear:
                    //This Year
                    From = DateTime.Now.Date.AddDays(-DateTime.Now.DayOfYear + 1);
                    Till = From.Value.AddYears(1).AddDays(-1);
                    break;
                case (int)Enums.DatingFilter.lastYear:
                    //Previous Year
                    From = DateTime.Now.Date.AddYears(-1).AddDays(-DateTime.Now.DayOfYear + 1);
                    Till = From.Value.Date.AddYears(1).AddDays(-1);
                    break;

                    

            }

                        #endregion
            if (Till.HasValue)
            {
                Till = Till.Value.AddMinutes(24 * 60);
            }

            return new Tuple<DateTime?, DateTime?>(From, Till);
        }

    }
}
