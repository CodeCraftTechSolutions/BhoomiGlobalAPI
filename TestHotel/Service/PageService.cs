using AutoMapper;
using BhoomiGlobal.Service.Extension;
using BhoomiGlobal.Service.Infrastructure;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Repository.Infrastructure;
using BhoomiGlobalAPI.Repository.IRepository;
using BhoomiGlobalAPI.Service.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;
namespace LogicLync.Service
{
    public class PageService:IPageService
    {
        IPageRepository _PageRepository;
        IPageImageRepository _PageImageRepository;
        private readonly IPageSectionService _pageSectionService;
        IPageCategoryRepository _pageCategoryRepository;
        //IMenuCategoryRepository _menuCategoryRepository;
        IWebSettingsRepository _webSettingRepository;
        IMapper _mapper; 
        IUnitOfWork _unitOfWork;
        public PageService(
            IPageRepository PageRepository,
            IPageCategoryRepository pageCategoryRepository,
            IWebSettingsRepository webSettingRepository,           
            IPageImageRepository PageImageRepository,  
            IPageSectionService pageSectionService,
            //IMenuCategoryRepository menuCategoryRepository,
            IUnitOfWork unitOfWork, IMapper mapper
        )
        {
            _PageRepository = PageRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _PageImageRepository = PageImageRepository;
            _pageSectionService = pageSectionService;
            _pageCategoryRepository = pageCategoryRepository;
            _webSettingRepository = webSettingRepository;
            //_menuCategoryRepository = menuCategoryRepository;
        }
        public PageBundle GetAllForWeb()
        {
            var webSetting =  _webSettingRepository.All.FirstOrDefault();
            if (webSetting == null) return new PageBundle();
            var pageCategory = _pageCategoryRepository.FindBy(x => x.CategoryCode == webSetting.OurServiceCategoryId).FirstOrDefault();
            if (pageCategory != null)
            {
                var pageCategoryId = pageCategory.Id;
                var _Pages = _PageRepository.FindBy(x => x.PageCategoryId == webSetting.OurServiceCategoryId);
                var Pages = _Pages.Select(ll => new PageViewModel
                                {
                                    Id = ll.Id,
                                    ImageUrl = "UploadsPageImage"+"//"+ ll.PageImages.Where(x => x.PageId == ll.Id  && x.IsPrimary == true).Select(x => x.ImagePath).First(),
                                    LongDescription = ll.LongDescription,
                                    ShortDescription = ll.ShortDescription,
                                    Title = ll.Title
                                });
                return new PageBundle()
                {
                    PageCategoryId = pageCategoryId,
                    Description = pageCategory.Description,
                    PageCategoryName = pageCategory.Name,
                    PageModifiedDTOs = Pages
                };
            }
            return new PageBundle();
        }
        public PageDTO GetByIdForWeb(int id)
        {
            var Page = _PageRepository.All
                        .Include(x => x.PageImages)
                        .Include(x=>x.PageCategory)
                        .FirstOrDefault(x=>x.Id==id);
            var _Page = new PageDTO()
            {
                Id = Page.Id,
                PageName = Page.PageName,
                PageCategoryName = Page.PageCategory.Name,
                LongDescription = Page.LongDescription,
                ShortDescription = Page.ShortDescription,
                Title = Page.Title,
                PageImages = _mapper.Map<ICollection<PageImageDTO>>(Page.PageImages)
            };
            return _Page;
        }
        public IEnumerable<PageDTO> GetAll()
        {
            IEnumerable<Page> Pages = _PageRepository.GetAll().Include(x=>x.PageImages);
            return _mapper.Map<IEnumerable<PageDTO>>(Pages);
        }
        public async Task<int> Create(PageDTO model)
        {
            try
            {
                Page obj = _mapper.Map<Page>(model);
                obj.PageCategoryId = model.PageCategoryId ?? 0;
                obj.PageCategory = null;
                await _PageRepository.Add(obj);
                await _unitOfWork.Commit();
                if (obj.Id > 0 && model.SectionsDetailsList != null)
                {
                    var pageSection = model.SectionsDetailsList;
                    pageSection.PageId = obj.Id;
                    var result = await _pageSectionService.Create(pageSection);
                    if (result) return obj.Id;
                }
                return 0;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        public List<PageDTO> GetPageByPageCategoryId(int pageCategoryId)
        {
            List<Page> pages = _PageRepository.GetAll(a => a.PageCategoryId == pageCategoryId).ToList();
            var pagesDTO= _mapper.Map<List<PageDTO>>(pages);
            foreach(var item in pagesDTO)
            {
                item.Name = item.Title;
            }
            return pagesDTO;
        }
        public async Task<int> Update(PageDTO model)
        {

            var Page = await _PageRepository.GetSingle(model.Id);
            if (Page != null)
            {
                //Page.Id = model.Id;
                Page.Title = model.Title;
                Page.PageUrlCode = model.PageUrlCode;
                Page.ShortDescription = model.ShortDescription;
                Page.LongDescription = model.LongDescription;
                Page.PageCategoryId = model.PageCategoryId ?? 0;
                Page.PageName = model.PageName;
                Page.SmallBannerImgUrl = model.SmallBannerImgUrl;
                Page.BannerImgUrl = model.BannerImgUrl;

                if (Page.Id > 0 && model.SectionsDetailsList != null)
                {
                    var pageSection = model.SectionsDetailsList;
                    pageSection.PageId = Page.Id;
                    var result = await _pageSectionService.Update(pageSection);
                    if(result == false) return -1;
                }

                await _unitOfWork.Commit();
            }
            return Page.Id;
        }
        public async Task Delete(int Id)
        {
            var Page = await _PageRepository.GetSingle(Id);
            _PageRepository.Delete(Page);
            await _unitOfWork.Commit();
        }
        //public async Task<PageDTO> GetPageById(int id)
        //{
        //    var Pages = await _PageRepository.GetSingle(id);
        //    if (Pages == null) return new PageDTO();
        //    return _mapper.Map<PageDTO>(Pages);
        //}


        public async Task<PageDTO> GetPageById(int id)
        {
            // Get the page along with its related sections and section details
            var Page = _PageRepository.GetAll(x => x.Id == id)
                .Include(p =>p.PageSections) // Include PageSections
                    .ThenInclude(s => s.PageSectionDetailsList).FirstOrDefault();

            if (Page == null)
            {
                return new PageDTO();
            }

            // Map Page to PageDTO
            var PageDTO = _mapper.Map<PageDTO>(Page);

            // If you want to return nested PageSectionDTO and PageSectionDetailDTO
            PageDTO.SectionsDetailsList = new PageSectionUpsertDTO()
            {
                PageId = Page.Id,
                PageSectionInputList = Page.PageSections.Select(section => new PageSectionInputDTO
                {
                    Id = section.Id,
                    Title = section.Title,
                    PageId = section.PageId,
                    PageSectionDetailsList = section.PageSectionDetailsList.Select(x => new PageSectionDetailsDTO()
                    {
                        Id =x.Id,
                        PageSectionId = x.PageSectionId,
                        Title=x.Title,
                        SubTitle=x.SubTitle,
                        ShortDescription = x.ShortDescription,
                        Description=x.Description,
                        ImageUrl = x.ImageUrl,
                        IconUrl = x.IconUrl,
                        IsActive = x.IsActive
                    }).ToList()
                }).ToList()
            };
            return PageDTO;
        }


        public async Task<PageDTO> GetPageByCode(string code)
        {
            //var Pages = await _PageRepository.GetByIdAsync(x => x.PageUrlCode == code);
            //if (Pages == null) return new PageDTO();
            //return _mapper.Map<PageDTO>(Pages);


            var Page = _PageRepository.GetAll(x => x.PageUrlCode == code)
               .Include(p => p.PageSections) // Include PageSections
                   .ThenInclude(s => s.PageSectionDetailsList).FirstOrDefault();

            if (Page == null)
            {
                return new PageDTO();
            }

            // Map Page to PageDTO
            var PageDTO = _mapper.Map<PageDTO>(Page);

            // If you want to return nested PageSectionDTO and PageSectionDetailDTO
            PageDTO.SectionsDetailsList = new PageSectionUpsertDTO()
            {
                PageId = Page.Id,
                PageSectionInputList = Page.PageSections.Select(section => new PageSectionInputDTO
                {
                    Id = section.Id,
                    Title = section.Title,
                    PageId = section.PageId,
                    PageSectionDetailsList = section.PageSectionDetailsList.Select(x => new PageSectionDetailsDTO()
                    {
                        Id = x.Id,
                        PageSectionId = x.PageSectionId,
                        Title = x.Title,
                        SubTitle = x.SubTitle,
                        ShortDescription = x.ShortDescription,
                        Description = x.Description,
                        ImageUrl = x.ImageUrl,
                        IconUrl = x.IconUrl,
                        IsActive = x.IsActive
                    }).ToList()
                }).ToList()
            };
            return PageDTO;
        }
        public PageDTO GetPageById(int Id, bool isAuthorized)
        {
            throw new NotImplementedException();
        }
        public PageDTO GetPageById(int Id, Expression<Func<Page, bool>> where = null, params Expression<Func<Page, object>>[] includeExpressions)
        {
            throw new NotImplementedException();
        }
        public async Task<QueryResult<PageDTO>> PageList(QueryObjectPage query)
        {
            if (string.IsNullOrEmpty(query.SortBy))
            {
                query.SortBy = "Title";
            }
            var columnMap = new Dictionary<string, Expression<Func<Page, object>>>()
            {
                ["Id"] = p => p.Id,
                ["Title"] = p => p.Title,
                ["shortDescription"] = p => p.ShortDescription,
                ["longDescription"] = p => p.LongDescription,
                ["pageCategoryId"] = p => p.PageCategoryId,
                ["PageName"] = p => p.PageName,
                ["pageCategoryName"] =p =>p.PageCategory.Name
               };

            var pagecatogory = _PageRepository.AllIncluding(a => a.PageCategory);
            if (!string.IsNullOrEmpty(query.SearchString))
            {
                pagecatogory = pagecatogory.Where(x => x.Title.Trim().ToLower().Contains(query.SearchString.Trim().ToLower())
                                                    || x.ShortDescription.Trim().ToLower().Contains(query.SearchString.Trim().ToLower())
                                                    || x.LongDescription.Trim().ToLower().Contains(query.SearchString.Trim().ToLower()));
            }

            if (query.pageCategoryId > 0)
            {
                pagecatogory = pagecatogory.Where(a => a.PageCategoryId == query.pageCategoryId);
            }
            var result = await pagecatogory.ApplyOrdering(query, columnMap).ToListAsync();
            var filterdatacount = pagecatogory.Count();


            var pagination = _mapper.Map<List<PageDTO>>(result);

            var queryResult = new QueryResult<PageDTO>
            {
                TotalItems = pagecatogory.Count(),
                Items = pagination
            };
            return queryResult;

        }
        public string GeneratePdfTemplateString(QueryResult<PageDTO> pagecategory)
        {
            var sb = new StringBuilder();

            sb.Append(@"<html>
                            <head>
                                <h1>Page</h1>
                            </head>
                            <body>
                                <table align='center'>");
            sb.Append(@"<thead>
                            <tr>
                                        <th>Id</th>
                                        <th>PageName</th>
                                        <th>Title </th>
                                        <th>ShortDescription </th>
                                        <th>LongDescription </th>
                                        <th>PageCategoryName </th>
                                    </tr></thead>");
            foreach (var item in pagecategory.Items)
            {


                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                  </tr>", item.Id, item.Title, item.ShortDescription,item.LongDescription,item.PageCategoryName, item.PageName);
            }

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
        public async Task UpdatePageImage(PageImageDTO PageImageDTO)
        {
            var PageImage = await _PageImageRepository.GetSingle(PageImageDTO.Id);
            if (PageImage != null)
            {
                PageImage.PageId = PageImageDTO.PageId;
                PageImage.ImagePath = PageImageDTO.ImagePath;
                PageImage.IsPrimary = PageImageDTO.IsPrimary;
            }
            await _unitOfWork.Commit();
        }
        public async Task PatchPageImage(PageImageCheckPrimaryDTO Pageimagedto)
        {
            var images = this._PageImageRepository.FindBy(p => p.PageId == Pageimagedto.PageId).ToList();
            foreach (var image in images)
            {
                if (image.Id == Pageimagedto.Id)
                {
                    image.IsPrimary = true;
                }
                else
                {
                    image.IsPrimary = false;
                }
                await _unitOfWork.Commit();
            }
        }
        public async Task UploadImage(int PageId, List<string> filepath)
        {
            var Page = await _PageRepository.GetSingle(PageId);
            var list = new List<PageImage>();
            foreach (var item in filepath)
            {
                var image = new PageImage
                {
                    PageId = PageId,
                    ImagePath = item
                };
                list.Add(image);
            }
            Page.PageImages = list;
            await _unitOfWork.Commit();
        }
        public async Task DeleteImages(int PageId)
        {
            try
            {

                List<PageImage> PageImages = _PageImageRepository.FindBy(b => b.PageId == PageId).ToList();
                if (PageImages.Count != 0)
                {
                    foreach (var img in PageImages)
                    {
                        _PageImageRepository.Delete(img);
                    }
                    await _unitOfWork.Commit();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task DeleteImage(int Id)
        {
            PageImage PageImage = await _PageImageRepository.GetSingle(Id);
            _PageImageRepository.Delete(PageImage);
            await _unitOfWork.Commit();
        }
        public List<PageImage> GetPageImages(int PageId)
        {
            var image = _PageImageRepository.FindBy(x => x.PageId == PageId);
            return image != null ? image.ToList() : new List<PageImage>();
        }
        public async Task<PageImageDTO> GetPageImage(int id)
        {
            PageImage PageImage = await _PageImageRepository.GetSingle(id);
            return _mapper.Map<PageImageDTO>(PageImage);
        }
        public void SaveChanges()
        {
            this._unitOfWork.Commit();
        }

        #region FAQ
        public async Task<List<PageDTO>> GetFAQ()
        {
            var webSetting = await _webSettingRepository.All.FirstOrDefaultAsync();
            if (webSetting == null) return new List<PageDTO>();
            var Page = _PageRepository.FindBy(x => x.PageCategoryId == webSetting.FaqCategoryId).Include(x=>x.PageImages).Include(x => x.PageCategory);
            return _mapper.Map<List<PageDTO>>(Page);
        }
        #endregion

        #region Support
        //public async Task<PageDTO> Support()
        //{
        //    var webSetting =await _webSettingRepository.All.FirstOrDefaultAsync();
        //    if (webSetting == null) return new PageDTO();
        //    var Page = await _PageRepository.All.FirstOrDefaultAsync(x => x.Id == webSetting.SupportPageId);
        //    return _mapper.Map<PageDTO>(Page);
        //}
        public async Task<List<PageDTO>> Support()
        {
            var webSetting = await _webSettingRepository.All.FirstOrDefaultAsync();
            if (webSetting == null) return new List<PageDTO>();
            var Page = _PageRepository.FindBy(x => x.PageCategoryId == webSetting.SupportPageId).Include(x => x.PageImages).Include(x => x.PageCategory);
            return _mapper.Map<List<PageDTO>>(Page);
        }
        public async Task<PageDTO> AboutUs()
        {
            var webSetting = await _webSettingRepository.All.FirstOrDefaultAsync();
            if (webSetting == null) return new PageDTO();
            var Page =  _PageRepository.GetAll(x => x.Id == webSetting.AboutUsPageId).Include(x=>x.PageImages).FirstOrDefault();
            return _mapper.Map<PageDTO>(Page);
        }
        #endregion

        #region TermsAndConditions

        public async Task<PageDTO> TermsAndConditionsWeb()
        {
            var webSetting = await _webSettingRepository.All.FirstOrDefaultAsync();
            if (webSetting == null) return new PageDTO();
            webSetting.WebTCId = webSetting.WebTCId ?? 0;
            var Page = await _PageRepository.All.FirstOrDefaultAsync(x => x.Id == webSetting.WebTCId);
            return _mapper.Map<PageDTO>(Page);
        }
        #endregion


    }
}
