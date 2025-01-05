using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;

namespace BhoomiGlobalAPI.Service.IService
{
    public interface IEmailService
    {
        Boolean Send(String templateName, List<KeyNamePair> replaceableItems, String ToEmail, List<KeyNamePair> Attachments = null);
        Boolean Send(string EmailFrom, string Subject, String Htmlbody, String ToEmail, List<KeyNamePair> Attachments = null);

        List<KeyNamePair> ConvertToMailMergeList(EmailTemplate emailTemplate);
        void UpdateCustomerDetail(UserDetails userDetail, List<KeyNamePair> keyNamePairs);
        void UpdateUserDetail(UserDetails userDetail, List<KeyNamePair> keyNamePairs, string password = "");
        void UpdateFranchiseDetail(UserDetails userDetail, List<KeyNamePair> keyNamePairs);

        List<KeyNamePair> UpdateMailBody(List<KeyNamePair> keyNamePairs);
        List<KeyNamePair> UpdateSubject(List<KeyNamePair> keyNamePairs);
        String GetSubject(List<KeyNamePair> keyNamePairs);

        String GetBodyHTML(List<KeyNamePair> keyNamePairs);
    }
}
