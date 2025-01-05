using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;

namespace BhoomiGlobalAPI.Repository
{
    public class CarouselRepository: RepositoryBase<Carousel>, ICarouselRepository
    {
        public CarouselRepository(IDbFactory dbFactory) : base(dbFactory) 
        {
            
        }
    }
}
