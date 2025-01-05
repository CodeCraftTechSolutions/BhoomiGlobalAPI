using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;
using BhoomiGlobalAPI.Service.IService;
using System.Net.Mail;

namespace BhoomiGlobalAPI.Service
{
    public class EmailService: IEmailService
    {
        private IEmailTemplateRepository _emailTemplateRepository;
        IUnitOfWork _unitOfWork;
        public EmailService(IEmailTemplateRepository emailTemplateRepository, IUnitOfWork unitOfWork)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _unitOfWork = unitOfWork;
        }
        public Boolean Send(String templateName, List<KeyNamePair> replaceableItems, String ToEmail, List<KeyNamePair> Attachments = null)
        {
            EmailTemplate objtemplate = _emailTemplateRepository.GetAll(a => a.TemplateName == templateName).FirstOrDefault();
            String MessageBody = objtemplate.HTMLWrapper;
            String Subject = objtemplate.Subject;
            if (replaceableItems != null)
            {
                foreach (KeyNamePair item in replaceableItems)
                {
                    MessageBody = MessageBody.Replace(item.Name, item.Value);
                    Subject = Subject.Replace(item.Name, item.Value);
                }
            }

            // Plug in your email service here to send an email.
            MailMessage emailmessage = new MailMessage(objtemplate.FromName + " " + objtemplate.EmailFrom, ToEmail, Subject, MessageBody);
            if (Attachments != null)
            {
                foreach (KeyNamePair att in Attachments)
                {
                    //string fileserverpath = HttpContext.Current.Server.MapPath("~/" + att.Value);
                    string fileserverpath = "";
                    if (File.Exists(fileserverpath))
                    {
                        Stream filestream = File.OpenRead(fileserverpath);
                        emailmessage.Attachments.Add(new Attachment(filestream, att.Name));
                    }
                }
            }
            emailmessage.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();

            smtp.Send(emailmessage);
            //return Task.FromResult(0);

            return true;
        }

        public Boolean Send(string EmailFrom, string Subject, String Htmlbody, String ToEmail, List<KeyNamePair> Attachments = null)
        {
            // Plug in your email service here to send an email.
            MailMessage emailmessage = new MailMessage(EmailFrom, ToEmail, Subject, Htmlbody);
            if (Attachments != null)
            {
                foreach (KeyNamePair att in Attachments)
                {
                    //string fileserverpath = HttpContext.Current.Server.MapPath("~/" + att.Value);
                    string fileserverpath = "";
                    if (File.Exists(fileserverpath))
                    {
                        Stream filestream = File.OpenRead(fileserverpath);
                        emailmessage.Attachments.Add(new Attachment(filestream, att.Name));
                    }
                }
            }
            emailmessage.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();

            smtp.Send(emailmessage);
            //return Task.FromResult(0);

            return true;
        }

        /// <summary>
        /// It assigns default values from template to create Merge Lists
        /// </summary>
        /// <param name="emailTemplate">Object of Type EmailTemplate</param>
        /// <returns></returns>
        public List<KeyNamePair> ConvertToMailMergeList(EmailTemplate emailTemplate)
        {
            List<KeyNamePair> lst = new List<KeyNamePair>();
            lst.Add(new KeyNamePair() { Name = "USERNAME", Value = "" });
            lst.Add(new KeyNamePair() { Name = "PASSWORD", Value = "" });
            lst.Add(new KeyNamePair() { Name = "FNAME", Value = "" });
            lst.Add(new KeyNamePair() { Name = "LNAME", Value = "" });
            lst.Add(new KeyNamePair() { Name = "EMAIL", Value = "" });
            lst.Add(new KeyNamePair() { Name = "JOB_NO", Value = "" });
            lst.Add(new KeyNamePair() { Name = "JOB_DATE", Value = "" });
            lst.Add(new KeyNamePair() { Name = "JOB_ADDRESS", Value = "" });
            lst.Add(new KeyNamePair() { Name = "JOB_SUMMARY", Value = "" });
            lst.Add(new KeyNamePair() { Name = "JOB_ACCEPTANCENOTE", Value = "" });

            lst.Add(new KeyNamePair() { Name = "MAILBODY", Value = emailTemplate.HTMLWrapper });
            lst.Add(new KeyNamePair() { Name = "MAILSUBJECT", Value = emailTemplate.Subject });

            lst.Add(new KeyNamePair() { Name = "CUSTOMER_FIRSTNAME", Value = "" });
            lst.Add(new KeyNamePair() { Name = "CUSTOMER_LASTNAME", Value = "" });
            lst.Add(new KeyNamePair() { Name = "CUSTOMER_PHONE", Value = "" });
            lst.Add(new KeyNamePair() { Name = "CUSTOMER_EMAIL", Value = "" });

            lst.Add(new KeyNamePair() { Name = "FRANCHISEE_FIRSTNAME", Value = "" });
            lst.Add(new KeyNamePair() { Name = "FRANCHISEE_LASTNAME", Value = "" });
            lst.Add(new KeyNamePair() { Name = "FRANCHISEE_PHONE", Value = "" });
            lst.Add(new KeyNamePair() { Name = "FRANCHISEE_EMAIL", Value = "" });

            lst.Add(new KeyNamePair() { Name = "SUBJECT", Value = "" });

            return lst;
        }
        /// <summary>
        /// Replaces all User related keys with values from User Detail
        /// </summary>
        /// <param name="userDetail"></param>
        /// <param name="keyNamePairs"></param>
        /// <param name="password"></param>
        public void UpdateUserDetail(UserDetails userDetail, List<KeyNamePair> keyNamePairs, string password = "")
        {
            try
            {
                KeyNamePair userName = keyNamePairs.FirstOrDefault(a => a.Name == "USERNAME");
                if (userName != null)
                {
                    userName.Value = userDetail.Email;
                }

                KeyNamePair fName = keyNamePairs.FirstOrDefault(a => a.Name == "FNAME");
                if (fName != null)
                {
                    fName.Value = string.IsNullOrEmpty(userDetail.FName) || userDetail.FName == userDetail.Email ? "There" : userDetail.FName;
                }

                KeyNamePair lName = keyNamePairs.FirstOrDefault(a => a.Name == "LNAME");
                if (lName != null)
                {
                    lName.Value = userDetail.LName;
                }

                KeyNamePair email = keyNamePairs.FirstOrDefault(a => a.Name == "EMAIL");
                if (email != null)
                {
                    email.Value = userDetail.Email;
                }

                KeyNamePair passwordKey = keyNamePairs.FirstOrDefault(a => a.Name == "PASSWORD");
                if (passwordKey != null)
                {
                    passwordKey.Value = password;
                }

                //KeyNamePair subjectKey = keyNamePairs.FirstOrDefault(a => a.Name == "SUBJECT");
                //if (subjectKey != null)
                //{
                //    subjectKey.Value = subject;
                //}
            }
            catch (Exception)
            {

            }

        }
        public void UpdateCustomerDetail(UserDetails userDetail, List<KeyNamePair> keyNamePairs)
        {
            try
            {
                KeyNamePair userName = keyNamePairs.FirstOrDefault(a => a.Name == "CUSTOMER_FIRSTNAME");
                if (userName != null)
                {
                    userName.Value = userDetail.FName;
                }

                KeyNamePair lName = keyNamePairs.FirstOrDefault(a => a.Name == "CUSTOMER_LASTNAME");
                if (lName != null)
                {
                    lName.Value = userDetail.LName;
                }

                KeyNamePair email = keyNamePairs.FirstOrDefault(a => a.Name == "CUSTOMER_EMAIL");
                if (email != null)
                {
                    email.Value = userDetail.Email;
                }
                KeyNamePair phone = keyNamePairs.FirstOrDefault(a => a.Name == "CUSTOMER_PHONE");
                if (phone != null)
                {
                    phone.Value = userDetail.PhoneNumber;
                }
            }
            catch (Exception)
            {

            }

        }
        public void UpdateFranchiseDetail(UserDetails userDetail, List<KeyNamePair> keyNamePairs)
        {
            try
            {
                KeyNamePair userName = keyNamePairs.FirstOrDefault(a => a.Name == "FRANCHISEE_FIRSTNAME");
                if (userName != null)
                {
                    userName.Value = userDetail.FName;
                }

                KeyNamePair lName = keyNamePairs.FirstOrDefault(a => a.Name == "FRANCHISEE_LASTNAME");
                if (lName != null)
                {
                    lName.Value = userDetail.LName;
                }

                KeyNamePair email = keyNamePairs.FirstOrDefault(a => a.Name == "FRANCHISEE_EMAIL");
                if (email != null)
                {
                    email.Value = userDetail.Email;
                }
                KeyNamePair phone = keyNamePairs.FirstOrDefault(a => a.Name == "FRANCHISEE_PHONE");
                if (phone != null)
                {
                    phone.Value = userDetail.PhoneNumber;
                }
            }
            catch (Exception)
            {

            }

        }

        public List<KeyNamePair> UpdateMailBody(List<KeyNamePair> keyNamePairs)
        {
            KeyNamePair mailBody = keyNamePairs.FirstOrDefault(a => a.Name == "MAILBODY");
            if (mailBody != null)
            {
                foreach (KeyNamePair item in keyNamePairs)
                {
                    mailBody.Value = mailBody.Value.Replace("*|" + item.Name + "|*", item.Value);

                }
            }
            return keyNamePairs;
        }
        public List<KeyNamePair> UpdateSubject(List<KeyNamePair> keyNamePairs)
        {
            KeyNamePair mailSubject = keyNamePairs.FirstOrDefault(a => a.Name == "MAILSUBJECT");
            if (mailSubject != null)
            {
                foreach (KeyNamePair item in keyNamePairs)
                {
                    mailSubject.Value = mailSubject.Value.Replace("*|" + item.Name + "|*", item.Value);
                }
            }
            return keyNamePairs;
        }

        public String GetSubject(List<KeyNamePair> keyNamePairs)
        {
            KeyNamePair obj = keyNamePairs.FirstOrDefault(a => a.Name == "MAILSUBJECT");
            if (obj != null)
            {
                return obj.Value;
            }
            return "";
        }

        public String GetBodyHTML(List<KeyNamePair> keyNamePairs)
        {
            KeyNamePair obj = keyNamePairs.FirstOrDefault(a => a.Name == "MAILBODY");
            if (obj != null)
            {
                return obj.Value;
            }
            return "";
        }
    }
}
