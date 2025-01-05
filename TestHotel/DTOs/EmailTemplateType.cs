using System.ComponentModel;

namespace BhoomiGlobalAPI.DTOs
{
    public enum EmailTemplateType
    {

        MEMBERREGISTRATION, // This Template is used to send the Email for Confirming email ID from user.
        MEMBERREGISTRATIONADMIN, // This Template is used to send the Email for Confirming email ID from user when created from admin.
        SUBADMINREGISTRATION,
        REGISTRATIONCONFIRMATION, // This Template is used to send an Acknowlegdement of Account Created Successfully.
        RESETPASSWORD, // This Template is used to Send email for resetpassword instruction( Email Confirmation button).
        PASSWORDRESETCONFIRMATION, // This template is used to send an Acknowledgement of Password changed successfully.
        PASSWORDRESETCONFIRMATIONAFFILIATE, //This template is used to send an acknowledger of paassword changed successfully for affiliate.
        NEWCUSTOMEREMAIL, //This email is sent to notify the controller for new customer
        NEWORDEREMAIL, //This email is sent to store & admin for new orders
        ORDERCONFIRMATIONEMAIL, //This email is sent to user for order confirmation
        ORDERSTATUSCHANGEEMAIL, //This email is sent to the user when status of order is changed.
        NEWSLETTER, //This email is sent to user about the newsletter

        //FrontEndMails
        USERVERIFICATION, // This email is sent to verify frontend user
        USERVERIFICATIONCONFIRM,//This email is sent after the verification of frontend user
        CHANGEPASSWORDSUCCESS, //This eamil is sent after the success of changing password

        ENROLLEDTOPACKAGE,
        NORMALRIDEBOOKONEWAY,
        NORMALRIDEBOOKTWOWAY,
        HOURRIDEBOOK,
        DAYRIDEBOOK,
        TOURBOOK,
        PAYMENTSUCESSFULLY,
        CASHONDELIVERY,
        ENQUIRYEMAIL,
        ENQUIRYEMAILFORUSER


    }

    public enum EmailPriority
    {
        [Description("High")]
        High = 10,
        [Description("Medium")]
        Medium = 20,
        [Description("Low")]
        Low = 30
    }

    public enum EmailStatus
    {
        Pending = 10,
        Sent = 20,
        Failed = 30,
        OnHold = 40,
    }
}
