using AutoMapper;
using BhoomiGlobal.Service.Extension;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Repository.IRepository;
using BhoomiGlobalAPI.Service.IService;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;

namespace BhoomiGlobalAPI.Service
{
    public class EmailQueueService : IEmailQueueService
    {
        IEmailQueueRepository _emailQueueRepository;
        IEmailTemplateRepository _emailTemplateRepository;
        IUserDetailsRepository _userDetailsRepository;
        ICommonService _commonService;
        private IConverter _converter;

        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        public EmailQueueService(IEmailQueueRepository emailQueueRepository,
            IEmailTemplateRepository emailTemplateRepository,
            IUserDetailsRepository userDetailsRepository,
            IMapper mapper, IUnitOfWork unitOfWork,
            IConverter converter,
            ICommonService commonService
            )
        {
            _emailQueueRepository = emailQueueRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _userDetailsRepository = userDetailsRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _commonService = commonService;
            _converter = converter;
        }

        public IEnumerable<EmailQueueDTO> GetAll()
        {
            IEnumerable<EmailQueue> emailQueues = _emailQueueRepository.GetAll();
            return _mapper.Map<IEnumerable<EmailQueueDTO>>(emailQueues);
        }

        public EmailTemplate GetEmailTemplateByTemplateName(string templateName)
        {
            var template = _emailTemplateRepository.All.FirstOrDefault(x => x.TemplateName.Trim() == templateName.Trim());
            if (template != null) return template;
            return new EmailTemplate();
        }
        public async Task<QueryResult<EmailQueueDTO>> EmailQueueList(EmailQuerySearchQueryObject query)
        {

            if (string.IsNullOrEmpty(query.SortBy))
            {
                query.SortBy = "Id";
            }
            var colomnMap = new Dictionary<string, Expression<Func<EmailQueue, object>>>()
            {
                ["Id"] = p => p.Id,
                ["TemplateName"] = p => p.TemplateName,
                ["EmailFrom"] = p => p.FromEmail,
                ["FromName"] = p => p.FromName,
                ["Subject"] = p => p.Subject,
                ["ToName"] = p => p.ToName,
                ["SentOn"] = p => p.SentOn,
                ["Cancelled"] = p => p.Cancelled,
                ["QueuedOn"] = p => p.QueuedOn,
                ["Status"] = p => p.StatusId


            };

            var emailqueus = _emailQueueRepository.GetAll();
            if (!string.IsNullOrEmpty(query.SearchString))
            {
                emailqueus = emailqueus.Where(x => x.TemplateName.ToLower().Contains(query.SearchString.ToLower())
                || x.FromEmail.ToLower().Contains(query.SearchString.ToLower()) || x.FromName.ToLower().Contains(query.SearchString)
                || x.Subject.ToLower().Contains(query.SearchString.ToLower()) || x.ToName.ToLower().Contains(query.SearchString.ToLower()));
            }
            if (query.StatusId > -1)
            {
                emailqueus = emailqueus.Where(x => x.StatusId == query.StatusId);
            }
            if (query.Cancelled > -1)
            {
                emailqueus = emailqueus.Where(x => x.Cancelled == (query.Cancelled == 1 ? true : false));
            }
            if (Convert.ToInt32(query.QueuedOn) != (int)Enums.DatingFilter.anytime)
            {
                Tuple<DateTime?, DateTime?> tupleQuedOn = _commonService.getDateRange(query.QueuedOn, query.QueuedStartDate, query.QueuedEndDate);

                emailqueus = emailqueus.Where(o => ((tupleQuedOn.Item1.HasValue == false || o.QueuedOn >= tupleQuedOn.Item1) && (
                 tupleQuedOn.Item2.HasValue == false || o.QueuedOn < tupleQuedOn.Item2)));
            }
            if (Convert.ToInt32(query.SentOn) != (int)Enums.DatingFilter.anytime)
            {
                Tuple<DateTime?, DateTime?> tupleSentOn = _commonService.getDateRange(query.SentOn, query.SentOnStartDate, query.SentOnEndDate);
                emailqueus = emailqueus.Where(o => ((tupleSentOn.Item1.HasValue == false || o.SentOn >= tupleSentOn.Item1) && (
                 tupleSentOn.Item2.HasValue == false || o.SentOn < tupleSentOn.Item2)));
            }


            var result = await emailqueus.ApplyOrdering(query, colomnMap).ToListAsync();
            int filterdatacount = emailqueus.Count();

            var pagination = _mapper.Map<List<EmailQueueDTO>>(result);
            var resultant = result.Select(x => new EmailQueueDTO
            {
                Id = x.Id,
                TemplateName = x.TemplateName,
                FromEmail = x.FromEmail,
                FromName = x.FromName,
                Subject = x.Subject,
                ToName = x.ToName,
                Cancelled = x.Cancelled,
                StatusId = x.StatusId,
                QueuedOn = x.QueuedOn,
                SentOn = x.SentOn

            }); ;

            var queryResult = new QueryResult<EmailQueueDTO>
            {
                TotalItems = emailqueus.Count(),
                Items = pagination
            };
            return queryResult;
        }
        public string GeneratePdfTemplateString(QueryResult<EmailQueueDTO> emailqueues)
        {
            var sb = new StringBuilder();

            sb.Append(@"<html>
                            <head>
                                <h1>Email Queue</h1>
                            </head>
                            <body>
                                <table align='center'>");
            sb.Append(@"<thead>
                            <tr>
                                        <th>Id</th>
                                        <th>Template Name</th>
                                        <th>Email From</th>
                                        <th>From Name</th>
                                        <th>To Name</th>
                                        <th>Subject</th>
                                        <th>Cancelled</th>
                                        <th>Status</th>
                                        <th>Queue On</th>
                                        <th>Sent On</th>
                                    </tr></thead>");
            foreach (var item in emailqueues.Items)
            {
                switch (item.StatusId)
                {
                    case 10:
                        item.Status = "Pending";
                        break;
                    case 20:
                        item.Status = "Failed";
                        break;
                    case 30:
                        item.Status = "Paid";
                        break;
                    case 40:
                        item.Status = "Refunded";
                        break;

                }

                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                    <td>{6}</td>
                                    <td>{7}</td>
                                    <td>{8}</td>
                                    <td>{9}</td>
                                  </tr>", item.Id, item.TemplateName, item.FromName, item.FromName, item.ToName, item.Subject, item.Cancelled, item.Status, item.QueuedOn, item.SentOn);
            }

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }


        public async Task<Int64> Create(EmailQueueDTO model)
        {
            EmailQueue obj = _mapper.Map<EmailQueue>(model);
            await _emailQueueRepository.Add(obj);
            await _unitOfWork.Commit();
            return obj.Id;
        }
        public async Task<Int64> Update(EmailQueueDTO model)
        {
            var emailQueue = await _emailQueueRepository.GetSingle(model.Id);
            if (emailQueue != null)
            {
                emailQueue.TemplateName = model.TemplateName;
                emailQueue.PriorityId = model.PriorityId;
                emailQueue.FromEmail = model.FromEmail;
                emailQueue.FromName = model.FromName;
                emailQueue.ToEmail = model.ToEmail;
                emailQueue.ToName = model.ToName;
                emailQueue.ReplyToEmail = model.ReplyToEmail;
                emailQueue.ReplyToName = model.ReplyToName;
                emailQueue.CcEmail = model.CcEmail;
                emailQueue.BccEmail = model.BccEmail;
                emailQueue.Subject = model.Subject;
                emailQueue.Body = model.Body;
                emailQueue.CallBackUrl = model.CallBackUrl;
                emailQueue.TempPassword = model.TempPassword;
                emailQueue.StatusId = model.StatusId;
                emailQueue.FailCount = model.FailCount;
                emailQueue.Attachements = model.Attachements;
                emailQueue.CreatedOn = model.CreatedOn;
                emailQueue.QueuedOn = model.QueuedOn;
                emailQueue.SentOn = model.SentOn;
                emailQueue.UserId = model.UserId;
                emailQueue.Cancelled = model.Cancelled;
                emailQueue.AddNotification = model.AddNotification;

                await _unitOfWork.Commit();
            }
            return emailQueue.Id;
        }
        public async Task Delete(Int64 id)
        {
            var emailQueue = await _emailQueueRepository.GetSingle(id);
            _emailQueueRepository.Delete(emailQueue);
            await _unitOfWork.Commit();
        }

        public async Task<EmailQueueDTO> GetEmailQueueById(Int64 id)
        {
            var country = await _emailQueueRepository.GetSingle(id);
            return _mapper.Map<EmailQueueDTO>(country);
        }

        public EmailQueueDTO GetEmailQueueById(Int64 Id, bool isAuthorized)
        {
            throw new NotImplementedException();
        }

        public EmailQueueDTO GetEmailQueueById(Int64 Id, Expression<Func<EmailQueue, bool>> where = null, params Expression<Func<EmailQueue, object>>[] includeExpressions)
        {
            throw new NotImplementedException();
        }

        public async Task ProcessEmailQueue(EmailServerSetting emailSetting)
        {
            IEnumerable<EmailQueue> emailQueues = _emailQueueRepository.GetAll().Where(x => x.StatusId == (int)EmailStatus.Pending);
            List<EmailQueueDTO> emailQueuesDto = _mapper.Map<IEnumerable<EmailQueueDTO>>(emailQueues).ToList();

            var webLink = "";
            var AdminLink = "";
            var PartnerLink = "";

            foreach (EmailQueueDTO item in emailQueuesDto)
            {

                EmailTemplate emailTemplate = _emailTemplateRepository.All.AsNoTracking().Where(x => x.TemplateName.ToLower() == item.TemplateName.ToString().ToLower()).FirstOrDefault();
                if (emailTemplate != null)
                {
                    List<KeyNamePair> keyNamePairs = ConvertToMailMergeList(emailTemplate);
                    UpdateContent(item.Body, keyNamePairs);
                    Int64 userId = item.UserId;


                    if (userId > 0)
                    {
                        UserDetails userDetail = await _userDetailsRepository.GetSingle(userId);
                        if (userDetail != null)
                        {
                            UpdateUserDetail(userDetail, keyNamePairs, item.TempPassword, item.CallBackUrl);
                        }
                    }

                    UpdateMailBody(keyNamePairs);

                    item.Body = GetBodyHTML(keyNamePairs);
                    item.Subject = GetSubject(keyNamePairs);
                    item.FromEmail = emailTemplate.EmailFrom;
                    item.FromName = emailTemplate.FromName;
                }

               /* await EmailAffiliate(item, emailTemplate)*/;
                var isSent = this.Send(emailSetting, item);
                if (isSent)
                {
                    item.StatusId = (int)EmailStatus.Sent;
                    item.SentOn = DateTime.Now;
                    await this.Update(item);
                }


            }
        }

        public async Task<bool> CancelEmailQueue(Int64 id)
        {
            if (id > 0)
            {
                List<EmailQueue> list = _emailQueueRepository.All.Where(x => x.Data == id.ToString()).ToList();
                if (list != null && list.Count > 0)
                {
                    foreach (EmailQueue item in list)
                    {
                        item.Cancelled = true;
                        item.CancelledOn = DateTime.Now;
                    }
                    return await _unitOfWork.Commit() > 0;
                }
                return false;
            }
            return false;
        }

        private bool Send(EmailServerSetting emailSetting, EmailQueueDTO emailDetial)
        {
            try
            {
                MailMessage emailmessage = new MailMessage(emailDetial.FromName + " " + emailDetial.FromEmail, emailDetial.ToEmail, emailDetial.Subject, emailDetial.Body);
                if (string.IsNullOrEmpty(emailDetial.CcEmail) == false)
                {
                    emailmessage.CC.Add(emailDetial.CcEmail);
                }
                if (string.IsNullOrEmpty(emailDetial.BccEmail) == false)
                {
                    emailmessage.Bcc.Add(emailDetial.BccEmail);
                }
                emailmessage.IsBodyHtml = true;

                if (!string.IsNullOrEmpty(emailDetial.Attachements))
                {
                    string[] splitFiles = emailDetial.Attachements.Split(';');
                    if (splitFiles != null && splitFiles.Length > 0)
                    {
                        foreach (var item in splitFiles)
                        {
                            emailmessage.Attachments.Add(new Attachment(item));
                        }
                    }
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Host = emailSetting.Host; //"smtp.gmail.com";
                smtp.Port = emailSetting.Port; //587;
                smtp.EnableSsl = emailSetting.EnableSsl; //true;
                smtp.Credentials = new System.Net.NetworkCredential(emailSetting.UserName, emailSetting.Password); //new System.Net.NetworkCredential("kinnidev@gmail.com", "Aabc123#");
                smtp.Send(emailmessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void SaveChanges()
        {
            this._unitOfWork.Commit();
        }

        public void UpdateUserDetail(UserDetails userDetail, List<KeyNamePair> keyNamePairs, string password = "", string callBackUrl = "")
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

                KeyNamePair PHONE = keyNamePairs.FirstOrDefault(a => a.Name == "PHONE");
                if (PHONE != null)
                {
                    PHONE.Value = userDetail.PhoneNumber;
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

                KeyNamePair callBackUrlKey = keyNamePairs.FirstOrDefault(a => a.Name == "CALLBACKURL");
                if (callBackUrlKey != null)
                {
                    callBackUrlKey.Value = callBackUrl;
                }
            }
            catch (Exception)
            {
            }
        }


        private List<KeyNamePair> ConvertToMailMergeList(EmailTemplate emailTemplate)
        {
            List<KeyNamePair> lst = new List<KeyNamePair>();
            lst.Add(new KeyNamePair() { Name = "USERNAME", Value = "" });
            lst.Add(new KeyNamePair() { Name = "PASSWORD", Value = "" });
            lst.Add(new KeyNamePair() { Name = "FNAME", Value = "" });
            lst.Add(new KeyNamePair() { Name = "LNAME", Value = "" });
            lst.Add(new KeyNamePair() { Name = "PHONE", Value = "" });
            lst.Add(new KeyNamePair() { Name = "EMAIL", Value = "" });
            lst.Add(new KeyNamePair() { Name = "CALLBACKURL", Value = "" });

            //Ride Booking
            lst.Add(new KeyNamePair() { Name = "SERVICECLASS", Value = "" });
            lst.Add(new KeyNamePair() { Name = "SERVICETYPE", Value = "" });
            lst.Add(new KeyNamePair() { Name = "PICKUPLOCATION", Value = "" });
            lst.Add(new KeyNamePair() { Name = "DROPOFFLOCATION", Value = "" });
            lst.Add(new KeyNamePair() { Name = "TRAVELDATE", Value = "" });
            lst.Add(new KeyNamePair() { Name = "TIME", Value = "" });
            lst.Add(new KeyNamePair() { Name = "BOOKFOROTHER", Value = "" });

            //Return Booking
            lst.Add(new KeyNamePair() { Name = "RETURNPICKUPLOCATION", Value = "" });
            lst.Add(new KeyNamePair() { Name = "RETURNDROPOFFLOCATION", Value = "" });
            lst.Add(new KeyNamePair() { Name = "RETURNDATE", Value = "" });
            lst.Add(new KeyNamePair() { Name = "RETURNTIME", Value = "" });

            //Additional Information
            lst.Add(new KeyNamePair() { Name = "PASSENGERNAME", Value = "" });
            lst.Add(new KeyNamePair() { Name = "NUMOFPASSENGER", Value = "" });
            lst.Add(new KeyNamePair() { Name = "NUMOFBAGGAGES", Value = "" });
            lst.Add(new KeyNamePair() { Name = "PICKUPSIGN", Value = "" });
            lst.Add(new KeyNamePair() { Name = "FLIGHTDETAILS", Value = "" });
            lst.Add(new KeyNamePair() { Name = "SPECIALNOTES", Value = "" });

            //Hour Ride
            lst.Add(new KeyNamePair() { Name = "DURATION", Value = "" });

            //DAY RIDE
            lst.Add(new KeyNamePair() { Name = "CITYTYPE", Value = "" });

            //Payment 
            lst.Add(new KeyNamePair() { Name = "PAYMENTDATE", Value = "" });
            lst.Add(new KeyNamePair() { Name = "TOTALAMOUNT", Value = "" });
            lst.Add(new KeyNamePair() { Name = "BOOKINGID", Value = "" });


            //TOUR BOOKING
            lst.Add(new KeyNamePair() { Name = "TOURNAME", Value = "" });
            lst.Add(new KeyNamePair() { Name = "TOURDATE", Value = "" });
            lst.Add(new KeyNamePair() { Name = "SPECIALREQUEST", Value = "" });

            //LINK
            lst.Add(new KeyNamePair() { Name = "LINK", Value = "" });
            lst.Add(new KeyNamePair() { Name = "WEBLINK", Value = "" });
            lst.Add(new KeyNamePair() { Name = "PARTNERLINK", Value = "" });
            lst.Add(new KeyNamePair() { Name = "ADMINLINK", Value = "" });

            //Password Reset Link
            lst.Add(new KeyNamePair() { Name = "WEBRESETLINK", Value = "" });
            lst.Add(new KeyNamePair() { Name = "PARTNERRESETLINK", Value = "" });
            lst.Add(new KeyNamePair() { Name = "ADMINRESETLINK", Value = "" });


            lst.Add(new KeyNamePair() { Name = "LOGO", Value = "" });


            lst.Add(new KeyNamePair() { Name = "MAILBODY", Value = emailTemplate.HTMLWrapper });
            lst.Add(new KeyNamePair() { Name = "MAILSUBJECT", Value = emailTemplate.Subject });
            lst.Add(new KeyNamePair() { Name = "CONTENT", Value = "" });

            lst.Add(new KeyNamePair() { Name = "AFFILIATESUPPORTCOMMENT", Value = "" });
            lst.Add(new KeyNamePair() { Name = "AFFILIATESUPPORTSUBJECT", Value = "" });

            return lst;
        }

        private void UpdateContent(string bodyContent, List<KeyNamePair> keyNamePairs)
        {
            try
            {
                KeyNamePair content = keyNamePairs.FirstOrDefault(a => a.Name == "CONTENT");
                if (content != null)
                {
                    content.Value = bodyContent;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private List<KeyNamePair> UpdateMailBody(List<KeyNamePair> keyNamePairs)
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

        private List<KeyNamePair> UpdateSubject(List<KeyNamePair> keyNamePairs)
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
        public async Task<bool> ChangeStatus(Int64 id)
        {
            try
            {
                EmailQueue obj = await _emailQueueRepository.GetSingle(id);
                if (obj != null)
                {
                    obj.Cancelled = !obj.Cancelled;
                    return await _unitOfWork.Commit() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task InsertoEmailQueue(UserDetailsDTO userModel, string template, string callbackurl = null, string data = "")
        {
            var email = GetEmailTemplateByTemplateName(template);
            EmailQueueDTO queueModel = new EmailQueueDTO()
            {
                TemplateName = template,
                PriorityId = (int)EmailPriority.Medium,
                ToEmail = userModel.Email,
                ToName = userModel.FName + " " + userModel.LName,
                Subject = email.Subject,
                Body = email.HTMLWrapper,
                StatusId = (int)EmailStatus.Pending,
                Attachements = "",
                CreatedOn = DateTime.Now,
                QueuedOn = DateTime.Now,
                Data = data,
                UserId = userModel.Id,
                CallBackUrl = callbackurl ?? null,
                AddNotification = true,
                FromName = email.FromName,
                FromEmail = email.EmailFrom,
                //IsAdmin = userModel.IsAdmin,
                //Password = userModel.Password,
            };
            await Create(queueModel);
        }

    }

}
