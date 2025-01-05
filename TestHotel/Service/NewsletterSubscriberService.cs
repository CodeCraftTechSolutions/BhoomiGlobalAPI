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
    public class NewsletterSubscriberService:INewsletterSubscriberService
    {
        private readonly INewsletterSubscriberRepository _newsletterSubscriberRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public NewsletterSubscriberService(
            INewsletterSubscriberRepository newsletterSubscriberRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _newsletterSubscriberRepository = newsletterSubscriberRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Create(NewsletterSubscriberDTO model)
        {
            NewsletterSubscriber obj = _mapper.Map<NewsletterSubscriber>(model);
            obj.CreatedOn = DateTime.Now;
            obj.ModifiedOn = DateTime.Now;
            await _newsletterSubscriberRepository.Add(obj);
            await _unitOfWork.Commit();
            return obj.Id;
        }

        public async Task<bool> Delete(long id)
        {
            NewsletterSubscriber data = await _newsletterSubscriberRepository.GetSingle(id);
            if (data != null)
            {
                _newsletterSubscriberRepository.Delete(data);
                return await _unitOfWork.Commit() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(string email)
        {
            var data = _newsletterSubscriberRepository.FindBy(x => x.EmailAddress == email).ToList();
            if (data != null && data.Any())
            {
                _newsletterSubscriberRepository.DeleteRange(data);
                return await _unitOfWork.Commit() > 0;
            }
            return false;
        }

        public IEnumerable<NewsletterSubscriberDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<NewsletterSubscriberDTO> GetNewsletterSubscriberById(long id)
        {
            throw new NotImplementedException();
        }

        public NewsletterSubscriberDTO GetNewsletterSubscriberById(long Id, bool isAuthorized)
        {
            throw new NotImplementedException();
        }

        public NewsletterSubscriberDTO GetNewsletterSubscriberById(long Id, Expression<Func<NewsletterSubscriber, bool>> where = null, params Expression<Func<NewsletterSubscriber, object>>[] includeExpressions)
        {
            throw new NotImplementedException();
        }

        public async Task<QueryResult<NewsletterSubscriberDTO>> NewsletterSubscriberList(NewsletterSubscriberQueryObject queryObject)
        {
            if (string.IsNullOrEmpty(queryObject.SortBy))
            {
                queryObject.SortBy = "id";
            }
            var columnMap = new Dictionary<string, Expression<Func<NewsletterSubscriber, object>>>()
            {
                ["id"] = p => p.Id,
                ["firstName"] = p => p.FirstName,
                ["lastName"] = p => p.LastName,
                ["emailAddress"] = p => p.EmailAddress,
                ["iPAddress"] = p => p.IPAddress,
                ["status"] = p => p.Status,
                ["createdOn"] = p => p.CreatedOn
            };

            var subscribers = _newsletterSubscriberRepository.All;

            if (queryObject.Status > -1)
            {
                subscribers = subscribers.Where(x => x.Status == queryObject.Status);
            }

            if (string.IsNullOrEmpty(queryObject.SearchText) == false)
            {
                queryObject.SearchText = queryObject.SearchText.Trim().ToLower();
                subscribers = subscribers.Where(x => x.FirstName.ToLower().Contains(queryObject.SearchText) || x.LastName.ToLower().Contains(queryObject.SearchText) || x.EmailAddress.Contains(queryObject.SearchText));
            }

            var result = await subscribers.ApplyOrdering(queryObject, columnMap).ToListAsync();
            var filterdatacount = subscribers.Count();

            var pagination = _mapper.Map<List<NewsletterSubscriberDTO>>(result);

            var queryResult = new QueryResult<NewsletterSubscriberDTO>
            {
                TotalItems = subscribers.Count(),
                Items = pagination
            };
            return queryResult;
        }

        public async Task<long> Update(NewsletterSubscriberDTO model)
        {
            var newsletterSubscriber = await _newsletterSubscriberRepository.GetSingle(model.Id);
            if (newsletterSubscriber != null)
            {
                newsletterSubscriber.FirstName = model.FirstName;
                newsletterSubscriber.LastName = model.LastName;
                newsletterSubscriber.EmailAddress = model.EmailAddress;
                newsletterSubscriber.IPAddress = model.IPAddress;
                newsletterSubscriber.Status = model.Status ?? 0;
                newsletterSubscriber.ModifiedOn = DateTime.Now;
                newsletterSubscriber.ModifiedById = model.ModifiedById;
                await _unitOfWork.Commit();
            }
            return newsletterSubscriber.Id;
        }

        public async Task<NewsletterSubscriberDTO> GetNewsletterSubscriberByEmail(string Email)
        {
            NewsletterSubscriber data = _newsletterSubscriberRepository.All.Where(x => x.EmailAddress.ToLower() == Email.ToLower()).FirstOrDefault();
            return _mapper.Map<NewsletterSubscriberDTO>(data);
        }

        public async Task<bool> UpdateStatus(Int64 id)
        {
            var data = await _newsletterSubscriberRepository.GetSingle(id);
            if (data != null)
            {
                if (data.Status == (int)Enums.NewsletterSubscriberStatus.Active)
                {
                    data.Status = (int)Enums.NewsletterSubscriberStatus.Inactive;
                }
                else
                {
                    data.Status = (int)Enums.NewsletterSubscriberStatus.Active;
                }
                return await _unitOfWork.Commit() > 0;
            }
            return false;
        }
        public string GeneratePdfTemplateString(QueryResult<NewsletterSubscriberDTO> newlettersubscribers)
        {
            var sb = new StringBuilder();

            sb.Append(@"<html>
                            <head>
                                <h1>Newsletter Subscribers</h1>
                            </head>
                            <body>
                                <table align='center'>");
            sb.Append(@"<thead>
                            <tr>
                                        <th>Id</th>
                                        <th>First Name</th>
                                        <th>Last Name</th>
                                        <th>Email Address</th>
                                        <th>IP Address</th>
                                        <th>Status</th>
                                        <th>Subscribed On</th>
                                    </tr></thead>");
            foreach (var item in newlettersubscribers.Items)
            {


                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                    <td>{6}</td>
                                    
                                  </tr>", item.Id, item.FirstName, item.LastName, item.EmailAddress, item.IPAddress, item.Status, item.CreatedOn);
            }

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
    }
}
