using AutoMapper;
using BhoomiGlobal.Service.Extension;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Repository.IRepository;
using BhoomiGlobalAPI.Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;

namespace BhoomiGlobalAPI.Service
{
    public class EmailLogService:IEmailLogService
    {
        IEmailLogRepository _emailLogRepository;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        ICommonService _commonService;
        public EmailLogService(IEmailLogRepository emailLogRepository, IMapper mapper, IUnitOfWork unitOfWork, ICommonService commonService)
        {
            _emailLogRepository = emailLogRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _commonService = commonService;
        }
        public IEnumerable<EmailLogDTO> GetAll()
        {
            IEnumerable<EmailLog> emailLogs = _emailLogRepository.GetAll();
            return _mapper.Map<IEnumerable<EmailLogDTO>>(emailLogs);
        }

        public async Task<Int64> Create(EmailLogDTO model)
        {
            EmailLog obj = _mapper.Map<EmailLog>(model);
            await _emailLogRepository.Add(obj);
            await _unitOfWork.Commit();
            return obj.EmailLogId;
        }
        public async Task<Int64> Update(EmailLogDTO model)
        {
            var emailLog = await _emailLogRepository.GetSingle(model.EmailLogId);
            if (emailLog != null)
            {
                emailLog.Subject = model.Subject;
                emailLog.Body = model.Body;
                emailLog.ToEmail = model.ToEmail;
                emailLog.FromEmail = model.FromEmail;
                emailLog.SendCopy = model.SendCopy;
                emailLog.Attachments = model.Attachments;
                emailLog.BCC = model.BCC;
                emailLog.CC = model.CC;
                emailLog.SendById = model.SendById;
                emailLog.SendDate = model.SendDate;
                emailLog.IsSent = model.IsSent;
                emailLog.Error = model.Error;

                await _unitOfWork.Commit();
            }
            return emailLog.EmailLogId;
        } 
        public async Task<QueryResult<EmailLogDTO>> EmailLogList(EmailLogSearchQueryObject query)
        {
            if (string.IsNullOrEmpty(query.SortBy))
            {
                query.SortBy = "EmailLogId";
            }
            var colomnMap = new Dictionary<string, Expression<Func<EmailLog, object>>>()
            {
                ["EmailLogId"] = p => p.EmailLogId,
                ["Subject"] = p => p.Subject,
                ["ToEmail"] = p => p.ToEmail,
                ["FromEmail"] = p => p.FromEmail,
                ["BCC"] = p => p.BCC,
                ["CC"] = p => p.CC,
                ["SendDate"] = p => p.SendDate,
                ["IsSent"] = p => p.IsSent,
                ["Error"] = p => p.Error,


            };

            var emailogs = _emailLogRepository.GetAll();
            if (!string.IsNullOrEmpty(query.SearchString))
            {
                emailogs = emailogs.Where(x => x.Subject.ToLower().Contains(query.SearchString.ToLower())
                || x.ToEmail.ToLower().Contains(query.SearchString.ToLower()) || x.ToEmail.ToLower().Contains(query.SearchString)
                || x.FromEmail.ToLower().Contains(query.SearchString.ToLower()) || x.FromEmail.ToLower().Contains(query.SearchString.ToLower())
                || x.BCC.ToLower().Contains(query.SearchString) || x.CC.ToLower().Contains(query.SearchString) || x.Error.ToLower().Contains(query.SearchString)
                );
            }
            if (query.IsSent > -1)
            {
                emailogs = emailogs.Where(x => x.IsSent == (query.IsSent == 1 ? true : false));
            }
            if (Convert.ToInt32(query.SelectedOption) != (int)Enums.DatingFilter.anytime)
            {
                Tuple<DateTime?, DateTime?> tupleQuedOn = _commonService.getDateRange(query.SelectedOption, query.StartDate, query.EndDate);

                emailogs = emailogs.Where(o => ((tupleQuedOn.Item1.HasValue == false || o.SendDate >= tupleQuedOn.Item1) && (
                 tupleQuedOn.Item2.HasValue == false || o.SendDate < tupleQuedOn.Item2)));
            }
            //if (query.StartDate.Year > 1970 || query.EndDate.Year > 1970)
            //{
            //    emailogs = emailogs.Where(o => (query.StartDate <= o.SendDate && o.SendDate <= query.EndDate)
            //                           // || (query.StartDate <= o.EndDate && o.EndDate <= query.EndDate)
            //                       );
            //}

            var result = await emailogs.ApplyOrdering(query, colomnMap).ToListAsync();
            int filterdatacount = emailogs.Count();

            var pagination = _mapper.Map<List<EmailLogDTO>>(result);
            var resultant = result.Select(x => new EmailLogDTO
            {
                EmailLogId = x.EmailLogId,
                Subject = x.Subject,
                ToEmail = x.ToEmail,
                FromEmail = x.FromEmail,
                BCC = x.BCC,
                CC = x.CC,
                SendDate = x.SendDate,
                IsSent = x.IsSent,
                Error = x.Error

            }); ;

            var queryResult = new QueryResult<EmailLogDTO>
            {
                TotalItems = emailogs.Count(),
                Items = pagination
            };
            return queryResult;
        }
        public async Task Delete(Int64 id)
        {
            var emailLog = await _emailLogRepository.GetSingle(id);
            _emailLogRepository.Delete(emailLog);
            await _unitOfWork.Commit();
        }

        public async Task<EmailLogDTO> GetEmailLogById(Int64 id)
        {
            var country = await _emailLogRepository.GetSingle(id);
            return _mapper.Map<EmailLogDTO>(country);
        }

        public EmailLogDTO GetEmailLogById(Int64 Id, bool isAuthorized)
        {
            throw new NotImplementedException();
        }

        public EmailLogDTO GetEmailLogById(Int64 Id, Expression<Func<EmailLog, bool>> where = null, params Expression<Func<EmailLog, object>>[] includeExpressions)
        {
            throw new NotImplementedException();
        }
        public string GeneratePdfTemplateString(QueryResult<EmailLogDTO> emailLogs)
        {
            var sb = new StringBuilder();

            sb.Append(@"<html>
                            <head>
                                <h1>Email Log</h1>
                            </head>
                            <body>
                                <table align='center'>");
            sb.Append(@"<thead>
                            <tr>
                                        <th>ID</th>
                                        <th>Subject</th>
                                        <th>Email To</th>
                                        <th>Email From</th>
                                        <th>BCC</th>
                                        <th>CC</th>
                                        <th>Send Date</th>
                                        <th>Is Sent</th>
                                        <th>Error</th>
                                    </tr></thead>");
            foreach (var item in emailLogs.Items)
            {


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
                                  </tr>", item.EmailLogId, item.Subject, item.ToEmail, item.FromEmail, item.BCC, item.CC, item.SendDate, item.IsSent, item.Error);
            }

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }

        public void SaveChanges()
        {
            this._unitOfWork.Commit();
        }
    }
}
