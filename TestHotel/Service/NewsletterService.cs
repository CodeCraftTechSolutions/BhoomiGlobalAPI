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
    public class NewsletterService:INewsletterService
    {
        private readonly INewsletterRepository _newsletterRepository;
        private readonly INewsletterSubscriberRepository _newsletterSubscriberRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public NewsletterService(
            INewsletterRepository newsletterRepository,
            INewsletterSubscriberRepository newsletterSubscriberRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _newsletterRepository = newsletterRepository;
            _newsletterSubscriberRepository = newsletterSubscriberRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<long> Create(NewsletterDTO model)
        {
            Newsletter obj = _mapper.Map<Newsletter>(model);
            obj.CreatedOn = DateTime.Now;
            obj.ModifiedOn = DateTime.Now;
            await _newsletterRepository.Add(obj);
            await _unitOfWork.Commit();
            return obj.Id;
        }

        public async Task<bool> Delete(long id)
        {
            Newsletter data = await _newsletterRepository.GetSingle(id);
            if (data != null)
            {
                _newsletterRepository.Delete(data);
                return await _unitOfWork.Commit() > 0;
            }
            return false;
        }

        public IEnumerable<NewsletterDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<NewsletterDTO> GetNewsletterByEmail(string Email)
        {
            throw new NotImplementedException();
        }

        public async Task<NewsletterDTO> GetNewsletterById(long id)
        {
            var newsletter = await _newsletterRepository.GetSingle(id);
            if (newsletter == null) return new NewsletterDTO();
            return _mapper.Map<NewsletterDTO>(newsletter);
        }

        public NewsletterDTO GetNewsletterById(long Id, bool isAuthorized)
        {
            throw new NotImplementedException();
        }

        public NewsletterDTO GetNewsletterById(long Id, System.Linq.Expressions.Expression<Func<Newsletter, bool>> where = null, params System.Linq.Expressions.Expression<Func<Newsletter, object>>[] includeExpressions)
        {
            throw new NotImplementedException();
        }

        public async Task<QueryResult<NewsletterDTO>> NewsletterList(NewsletterQueryObject queryObject)
        {
            if (string.IsNullOrEmpty(queryObject.SortBy))
            {
                queryObject.SortBy = "id";
            }
            var columnMap = new Dictionary<string, Expression<Func<Newsletter, object>>>()
            {
                ["id"] = p => p.Id,
                ["name"] = p => p.Name,
                ["description"] = p => p.Description,
                ["status"] = p => p.Status,
                ["sendOn"] = p => p.SendOn
            };

            var newsletters = _newsletterRepository.All;

            if (queryObject.Status > -1)
            {
                newsletters = newsletters.Where(x => x.Status == queryObject.Status);
            }

            if (string.IsNullOrEmpty(queryObject.SearchText) == false)
            {
                queryObject.SearchText = queryObject.SearchText.Trim().ToLower();
                newsletters = newsletters.Where(x => x.Name.ToLower().Contains(queryObject.SearchText) || x.Description.ToLower().Contains(queryObject.SearchText));
            }

            var result = await newsletters.ApplyOrdering(queryObject, columnMap).ToListAsync();
            var filterdatacount = newsletters.Count();

            var pagination = _mapper.Map<List<NewsletterDTO>>(result);

            var queryResult = new QueryResult<NewsletterDTO>
            {
                TotalItems = newsletters.Count(),
                Items = pagination
            };
            return queryResult;
        }

        public async Task<long> Update(NewsletterDTO model)
        {
            var newsletter = await _newsletterRepository.GetSingle(model.Id);
            if (newsletter != null)
            {
                newsletter.Name = model.Name;
                newsletter.Description = model.Description;
                newsletter.Subject = model.Subject;
                newsletter.EmailContent = model.EmailContent;
                newsletter.Status = model.Status;
                newsletter.SendOn = model.SendOn;
                newsletter.ModifiedOn = DateTime.Now;
                newsletter.ModifiedById = model.ModifiedById;
                await _unitOfWork.Commit();
            }
            return newsletter.Id;
        }

        public async Task<NewsletterAPIDTO> GetNewletterForAPI(Int64 id)
        {
            NewsletterAPIDTO data = new NewsletterAPIDTO();

            var subscribers = _newsletterSubscriberRepository.GetAll();
            var newsletter = await _newsletterRepository.GetSingle(id);
            if (subscribers != null && subscribers.Count() > 0 && newsletter != null)
            {
                data.Id = newsletter.Id;
                data.Subject = newsletter.Subject;
                data.Body = newsletter.EmailContent;
                data.Subscribers = (from s in subscribers
                                    select new NewsletterSubscriberAPIDTO()
                                    {
                                        Name = s.FirstName + " " + s.LastName,
                                        Email = s.EmailAddress
                                    }).ToList();
                data.KeyNamePairs = new List<KeyNamePair>();
                data.KeyNamePairs.Add(new KeyNamePair() { Name = "CONTENT", Value = newsletter.EmailContent });
                data.KeyNamePairs.Add(new KeyNamePair() { Name = "DATE", Value = newsletter.SendOn.ToLongDateString() });
            }
            return data;
        }
        public string GeneratePdfTemplateString(QueryResult<NewsletterDTO> newsletters)
        {
            var sb = new StringBuilder();

            sb.Append(@"<html>
                            <head>
                                <h1>NewsLetters</h1>
                            </head>
                            <body>
                                <table align='center'>");
            sb.Append(@"<thead>
                            <tr>
                                        <th>Id</th>
                                        <th>Name</th>
                                        <th>Description</th>
                                        <th>Status</th>
                                        <th>Send On</th>
                                     </tr></thead>");
            foreach (var item in newsletters.Items)
            {
                switch (item.Status)
                {
                    case 10:
                        item.statusString = "Draft";
                        break;
                    case 20:
                        item.statusString = "Confirmed";
                        break;
                    case 30:
                        item.statusString = "SendNow";
                        break;
                    case 40:
                        item.statusString = "Cancelled";
                        break;
                    case 50:
                        item.statusString = "Sent";
                        break;

                }

                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    
                                  </tr>", item.Id, item.Name, item.Description, item.statusString, item.SendOn);
            }

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
    }
}
