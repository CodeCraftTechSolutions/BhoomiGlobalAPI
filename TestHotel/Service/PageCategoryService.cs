using AutoMapper;
using BhoomiGlobal.Service.Extension;
using BhoomiGlobal.Service.Infrastructure;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Repository.IRepository;
using BhoomiGlobalAPI.Service.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;


namespace BhoomiGlobal.Service
{
    public class PageCategoryService:IPageCategoryService
    {
        IPageCategoryRepository _pageCategoryRepository;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        public PageCategoryService(
                IPageCategoryRepository pageCategoryRepository,
                IMapper mapper,
                IUnitOfWork unitOfWork
            )
        {
            _pageCategoryRepository = pageCategoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PageCategoryDTO> GetAll()
        {
            IEnumerable<PageCategory> pageCategories = _pageCategoryRepository.GetAll();
            return _mapper.Map<IEnumerable<PageCategoryDTO>>(pageCategories);
        }
        public async Task<int> Create(PageCategoryDTO model)
        {
           // model.CategoryCode = model.CategoryCode;
            PageCategory obj = _mapper.Map<PageCategory>(model);
            await _pageCategoryRepository.Add(obj);
            await _unitOfWork.Commit();
            return obj.Id;
        }
        public async Task<int> Update(PageCategoryDTO model)
        {

            var pageCategory = await _pageCategoryRepository.GetSingle(model.Id);
            if (pageCategory != null)
            {
                pageCategory.Id = model.Id;
                pageCategory.Name = model.Name;
                pageCategory.Description = model.Description;
               // pageCategory.CategoryCode = model.CategoryCode;
                await _unitOfWork.Commit();
            }
            return pageCategory.Id;
        }
        public async Task<PageCategoryDTO> GetPageCategoryById(int id)
        {
            var pageCategory = await _pageCategoryRepository.GetSingle(id);
            if (pageCategory == null) return new PageCategoryDTO();
            return _mapper.Map<PageCategoryDTO>(pageCategory);
        }
        public PageCategoryDTO GetPageCategoryById(int Id, bool isAuthorized) 
        {
            throw new NotImplementedException();
        }
        public PageCategoryDTO GetPageCategoryById(int Id, Expression<Func<PageCategory, bool>> where = null, params Expression<Func<PageCategory, object>>[] includeExpressions)
        {
            throw new NotImplementedException();
        }
        public async Task<QueryResult<PageCategoryDTO>> PageCategoryList(QueryObject query)
        {
            if (string.IsNullOrEmpty(query.SortBy))
            {
                query.SortBy = "Name";
            }
            var colomnMap = new Dictionary<string, Expression<Func<PageCategory, object>>>()
            {
                ["Id"] = p => p.Id,
                ["Name"] = p => p.Name,
                ["Description"] = p => p.Description,
                ["CategoryCode"] = p => p.CategoryCode,

            };

            var pageCategories = _pageCategoryRepository.GetAll();

            var result = await pageCategories.ApplyOrdering(query, colomnMap).ToListAsync();
            int filterdatacount = pageCategories.Count();

            var pagination = _mapper.Map<List<PageCategoryDTO>>(result);

            var queryResult = new QueryResult<PageCategoryDTO>
            {
                TotalItems = pageCategories.Count(),
                Items = pagination
            };
            return queryResult;
        }
        public string GeneratePdfTemplateString(QueryResult<PageCategoryDTO> pagecategory)
        {
            var sb = new StringBuilder();

            sb.Append(@"<html>
                            <head>
                                <h1>Page Category</h1>
                            </head>
                            <body>
                                <table align='center'>");
            sb.Append(@"<thead>
                            <tr>
                                        <th>Id</th>
                                        <th>Name</th>
                                        <th>Description</th>
                                    </tr></thead>");
            foreach (var item in pagecategory.Items)
            {
               

                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                  </tr>", item.Id, item.Name, item.Description);
            }

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
        public async Task Delete(int id)
        {
            var pageCategory = await _pageCategoryRepository.GetSingle(id);
            _pageCategoryRepository.Delete(pageCategory);
            await _unitOfWork.Commit();
        }
        public bool IsExisting(int pageCategoryCode)
        {
            bool checkExistence = _pageCategoryRepository.GetAll()
                .Select(x => x.CategoryCode)
                .Contains(pageCategoryCode);
            return checkExistence;
        }
        public void SaveChanges()
        {
            this._unitOfWork.Commit();
        }
        
    }
}
