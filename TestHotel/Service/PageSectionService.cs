using AutoMapper;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;
using BhoomiGlobalAPI.Service.Infrastructure;

namespace BhoomiGlobalAPI.Service
{
    public class PageSectionService : IPageSectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPageSectionDetailsRepository _pageSectionDetailsRepository;
        private readonly IPageSectionRepository _pageSectionRepository;

        public PageSectionService(IPageSectionRepository pageSectionRepository, IUnitOfWork unitOfWork, IMapper mapper,
                                IPageSectionDetailsRepository pageSectionDetailsRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _pageSectionDetailsRepository = pageSectionDetailsRepository;
            _pageSectionRepository = pageSectionRepository;
        }

        public async Task<bool> Create(PageSectionUpsertDTO model)
        {
            if(model.PageId > 0)
            {
                List<PageSection> pageSectionData = _mapper.Map<List<PageSection>>(model.PageSectionInputList);
                foreach (PageSection pageSection in pageSectionData)
                {
                    pageSection.PageId = model.PageId;
                    await _pageSectionRepository.Add(pageSection);
                    await _unitOfWork.Commit();
                    if (pageSection.Id == 0) return false;
                }
                return true;
            }
            return false;
        }

        public async Task<bool> Update(PageSectionUpsertDTO model)
        {
            if (model.PageId <= 0)
                return false;

            // Fetch existing sections without eager loading details
            var existingSections = _pageSectionRepository
                .GetAll(x => x.PageId == model.PageId)
                .ToList();

            // Map the incoming data
            var incomingSections = _mapper.Map<List<PageSection>>(model.PageSectionInputList);

            foreach (var incomingSection in incomingSections)
            {
                incomingSection.PageId = model.PageId;

                // Check if section already exists
                var existingSection = existingSections.FirstOrDefault(es => es.Id == incomingSection.Id);

                if (existingSection != null)
                {
                    // Update properties of the existing section
                    existingSection.Title = incomingSection.Title;
                    existingSection.IsActive = incomingSection.IsActive;

                    // Process section details
                    foreach (var incomingDetail in incomingSection.PageSectionDetailsList)
                    {
                        // Fetch existing detail directly from the repository
                        var existingDetailList = _pageSectionDetailsRepository.GetAll(x => x.PageSectionId == incomingDetail.PageSectionId).ToList();
                        var existingDetail = existingDetailList.FirstOrDefault(x => x.Id == incomingDetail.Id);

                        if (existingDetail != null)
                        {
                            // Update existing detail properties
                            existingDetail.Title = incomingDetail.Title;
                            existingDetail.SubTitle = incomingDetail.SubTitle;
                            existingDetail.Description = incomingDetail.Description;
                            existingDetail.ImageUrl = incomingDetail.ImageUrl;
                            existingDetail.IconUrl = incomingDetail.IconUrl;
                            existingDetail.IsActive = incomingDetail.IsActive;
                        }
                        else
                        {
                            // Add new detail
                            existingSection.PageSectionDetailsList.Add(new PageSectionDetails
                            {
                                Title = incomingDetail.Title,
                                SubTitle = incomingDetail.SubTitle,
                                Description = incomingDetail.Description,
                                ImageUrl = incomingDetail.ImageUrl,
                                IconUrl = incomingDetail.IconUrl,
                                IsActive = incomingDetail.IsActive,
                                PageSectionId = existingSection.Id
                            });
                        }
                    }

                    // Remove section details not in incoming data
                    List<PageSectionDetails> removedData = existingSection.PageSectionDetailsList.Where(ed =>
                        !incomingSection.PageSectionDetailsList.Any(id => id.Id == ed.Id)).ToList();

                    _pageSectionDetailsRepository.DeleteRange(removedData);
                }
                else
                {
                    // Add new section
                    await _pageSectionRepository.Add(incomingSection);
                }
            }

            // Mark sections as deleted if not in incoming data
            foreach (var existingSection in existingSections)
            {
                if (!incomingSections.Any(z => z.Id == existingSection.Id))
                {
                    var sectionsToDelete = await _pageSectionRepository
                                            .GetSingle(existingSection.Id);

                    _pageSectionRepository.Delete(sectionsToDelete);
                }
            }

            // Commit all changes
            await _unitOfWork.Commit();

            return true;
        }

    }
}
