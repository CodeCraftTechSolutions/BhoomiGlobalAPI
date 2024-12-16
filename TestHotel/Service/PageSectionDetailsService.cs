using AutoMapper;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Repository.IRepository;

namespace BhoomiGlobalAPI.Service
{
    public class PageSectionDetailsService
    {
        private readonly IPageSectionDetailsRepository _pageSectionDetailsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PageSectionDetailsService(IPageSectionDetailsRepository pageSectionDetailsRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _pageSectionDetailsRepository = pageSectionDetailsRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


    }
}
