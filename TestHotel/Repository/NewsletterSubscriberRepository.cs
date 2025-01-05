using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;

namespace BhoomiGlobalAPI.Repository
{
    public class NewsletterSubscriberRepository : RepositoryBase<NewsletterSubscriber>, INewsletterSubscriberRepository
    {
        public NewsletterSubscriberRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
