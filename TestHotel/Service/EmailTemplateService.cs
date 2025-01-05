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
    public class EmailTemplateService:IEmailTemplateService
    {
        IEmailTemplateRepository _emailTemplateRepository;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        public EmailTemplateService(IEmailTemplateRepository emailTemplateRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<EmailTemplateDTO> GetAll()
        {
            IEnumerable<EmailTemplate> emailTemplates = _emailTemplateRepository.GetAll();
            return _mapper.Map<IEnumerable<EmailTemplateDTO>>(emailTemplates);
        }

        public async Task<int> Create(EmailTemplateDTO model)
        {
            EmailTemplate obj = _mapper.Map<EmailTemplate>(model);
            obj.CreatedDate = DateTime.Now;
            await _emailTemplateRepository.Add(obj);
            await _unitOfWork.Commit();
            return obj.EmailTemplateId;
        }
        public async Task<int> Update(EmailTemplateDTO model)
        {
            var emailTemplate = await _emailTemplateRepository.GetSingle(model.EmailTemplateId);
            if (emailTemplate != null)
            {
                emailTemplate.TemplateName = model.TemplateName;
                emailTemplate.EmailFrom = model.EmailFrom;
                emailTemplate.FromName = model.FromName;
                emailTemplate.Subject = model.Subject;
                emailTemplate.Message = model.Message;
                emailTemplate.HTMLWrapper = model.HTMLWrapper;
                // emailTemplate.CreatedDate = DateTime.Now;
                emailTemplate.Active = model.Active;

                await _unitOfWork.Commit();
            }
            return emailTemplate.EmailTemplateId;
        }
        
        public async Task<QueryResult<EmailTemplateDTO>> EmailTemplateList(EmailTemplateSearchQueryObject query)
        {
            if (string.IsNullOrEmpty(query.SortBy))
            {
                query.SortBy = "EmailTemplateId";
            }
            var colomnMap = new Dictionary<string, Expression<Func<EmailTemplate, object>>>()
            {
                ["EmailTemplateId"] = p => p.EmailTemplateId,
                ["TemplateName"] = p => p.TemplateName,
                ["EmailFrom"] = p => p.EmailFrom,
                ["FromName"] = p => p.FromName,
                ["Subject"] = p => p.Subject,
                ["Message"] = p => p.Message,
                ["HTMLWrapper"] = p => p.HTMLWrapper,
                ["CreatedDate"] = p => p.CreatedDate,
                ["Active"] = p => p.Active,


            };

            var emailtemplates = _emailTemplateRepository.GetAll();
            if (!string.IsNullOrEmpty(query.SearchString))
            {
                emailtemplates = emailtemplates.Where(x => x.TemplateName.ToLower().Contains(query.SearchString.ToLower())
                || x.EmailFrom.ToLower().Contains(query.SearchString.ToLower()) || x.FromName.ToLower().Contains(query.SearchString)
                || x.Subject.ToLower().Contains(query.SearchString.ToLower()));
            }
            if (query.Active > -1)
            {
                emailtemplates = emailtemplates.Where(x => x.Active == query.Active);
            }

            var result = await emailtemplates.ApplyOrdering(query, colomnMap).ToListAsync();
            int filterdatacount = emailtemplates.Count();

            var pagination = _mapper.Map<List<EmailTemplateDTO>>(result);
            var resultant = result.Select(x => new EmailTemplateDTO
            {
                EmailTemplateId = x.EmailTemplateId,
                TemplateName = x.TemplateName,
                EmailFrom = x.EmailFrom,
                Subject = x.Subject,
                Message = x.Message,
                HTMLWrapper = x.HTMLWrapper,
                Active = x.Active
            });

            var queryResult = new QueryResult<EmailTemplateDTO>
            {
                TotalItems = emailtemplates.Count(),
                Items = pagination
            };
            return queryResult;
        }

        public async Task Delete(int id)
        {
            var emailTemplate = await _emailTemplateRepository.GetSingle(id);
            _emailTemplateRepository.Delete(emailTemplate);
            await _unitOfWork.Commit();

        }

        public EmailTemplateDTO GetEmailTemplateByName(string name)
        {
            var emailTemplate = _emailTemplateRepository.GetAll().Where(x => x.TemplateName.ToLower() == name.Trim().ToLower()).FirstOrDefault();
            return _mapper.Map<EmailTemplateDTO>(emailTemplate);
        }

        public async Task<EmailTemplateDTO> GetEmailTemplateById(int id)
        {
            var country = await _emailTemplateRepository.GetSingle(id);
            return _mapper.Map<EmailTemplateDTO>(country);
        }

        public EmailTemplateDTO GetEmailTemplateById(int Id, bool isAuthorized)
        {
            throw new NotImplementedException();
        }

        public EmailTemplateDTO GetEmailTemplateById(int Id, Expression<Func<EmailTemplate, bool>> where = null, params Expression<Func<EmailTemplate, object>>[] includeExpressions)
        {
            throw new NotImplementedException();
        }

        public string UpdateEmailContent(List<KeyNamePair> keyNamePairs, string content)
        {
            if (keyNamePairs != null && keyNamePairs.Count() > 0)
            {
                foreach (KeyNamePair item in keyNamePairs)
                {
                    content = content.Replace("*|" + item.Name + "|*", item.Value);
                }
            }
            return content;
        }
        public async Task<bool> ChangeStatus(int id)
        {
            try
            {
                EmailTemplate obj = await _emailTemplateRepository.GetSingle(id);
                if (obj != null)
                {
                    if (obj.Active == (int)Enums.EmailTemplateStatus.Active)
                    {
                        obj.Active = (int)Enums.EmailTemplateStatus.Inactive;
                    }
                    else
                    {
                        obj.Active = (int)Enums.EmailTemplateStatus.Active;
                    }
                    return await _unitOfWork.Commit() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string GeneratePdfTemplateString(QueryResult<EmailTemplateDTO> emailqueues)
        {
            var sb = new StringBuilder();

            sb.Append(@"<html>
                            <head>
                                <h1>Email Template</h1>
                            </head>
                            <body>
                                <table align='center'>");
            sb.Append(@"<thead>
                            <tr>
                                        <th>Id</th>
                                        <th>Template Name</th>
                                        <th>Email From</th>
                                        <th>From Name</th>
                                        <th>Subject</th>
                                        <th>Active</th>
                                    </tr></thead>");
            foreach (var item in emailqueues.Items)
            {

                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                  </tr>", item.EmailTemplateId, item.TemplateName, item.EmailFrom, item.FromName, item.Subject, item.Active == 1 ? "Active" : "InActive");
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
