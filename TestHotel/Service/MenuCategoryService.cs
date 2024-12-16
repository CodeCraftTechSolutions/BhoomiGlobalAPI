using AutoMapper;
using BhoomiGlobal.Service.Extension;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Repository.IRepository;
using BhoomiGlobalAPI.Service.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;



namespace BhoomiGlobalAPI.Service
{
    public class MenuCategoryService:IMenuCategoryService

    {
        IMenuCategoryRepository _menuCategoryRepository;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        public MenuCategoryService(
                IMenuCategoryRepository  menuCategoryRepository,
                IMapper mapper,
                IUnitOfWork unitOfWork
            )
        {
            _menuCategoryRepository = menuCategoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public List<MenuCategoryDTO> menuCategory
        {
            get
            {
                var result = _menuCategoryRepository.All.OrderBy(x => x.Name).ToList();
                return _mapper.Map<List<MenuCategoryDTO>>(result);

            }
        }
        public IEnumerable<MenuCategoryDTO> GetAll()
        {
            IEnumerable<MenuCategory> menuCategories =  _menuCategoryRepository.GetAll();
            return _mapper.Map<IEnumerable<MenuCategoryDTO>>(menuCategories);
        }
        public async Task<int> Create(MenuCategoryDTO model,Int64 userId)
        {

            MenuCategory obj = _mapper.Map<MenuCategory>(model);
            obj.CreatedById = userId;
            obj.CreatedOn = DateTime.Now;
            obj.ModifiedById = userId;
            obj.ModifiedOn = DateTime.Now;
            await _menuCategoryRepository.Add(obj);
            await _unitOfWork.Commit();
            return obj.Id;
        }
        public async Task<int> Update(MenuCategoryDTO model,Int64 userId)
        {

            var menuCategory = await _menuCategoryRepository.GetSingle(model.Id);
            if (menuCategory != null)
            {
                menuCategory.Id = model.Id;
                menuCategory.Name = model.Name;
                menuCategory.Status = model.Status;
                menuCategory.ModifiedById = userId;
                menuCategory.ModifiedOn = DateTime.Now;
                menuCategory.ParentId = model.ParentId;

                await _unitOfWork.Commit();
            }
            return menuCategory.Id;
        }
        public async Task<MenuCategoryDTO> GetMenuCategoryById(int id)
        {
            var menucategory = await _menuCategoryRepository.GetSingle(id);
            if (menucategory == null) return new MenuCategoryDTO();
            return _mapper.Map<MenuCategoryDTO>(menucategory);
        }
        public MenuCategoryDTO GetMenuCategoryById(int Id, bool isAuthorized)
        {
            throw new NotImplementedException();
        }
        public MenuCategoryDTO GetMenuCategoryById(int Id, Expression<Func<MenuCategory, bool>> where = null, params Expression<Func<MenuCategory, object>>[] includeExpressions)
        {
            throw new NotImplementedException();
        }
        public async Task<QueryResult<MenuCategoryDTO>> MenuCategoryList(QueryObject query)
        {
            if (string.IsNullOrEmpty(query.SortBy))
            {
                query.SortBy = "Name";
            }
            var colomnMap = new Dictionary<string, Expression<Func<MenuCategory, object>>>()
            {
                ["Id"] = p => p.Id,
                ["Name"] = p => p.Name,
                ["Status"] = p => p.Status,
                ["CreatedById"] =p=>p.CreatedById,
                ["CreatedOn"] =p=>p.CreatedOn,
                ["ModifiedById"] =p=>p.ModifiedById,
                ["ModifiedOn"] =p=>p.ModifiedOn

            };

            var menucategories = _menuCategoryRepository.GetAll();

            var result = await menucategories.ApplyOrdering(query, colomnMap).ToListAsync();
            int filterdatacount = menucategories.Count();

            var pagination = _mapper.Map<List<MenuCategoryDTO>>(result);

            var queryResult = new QueryResult<MenuCategoryDTO>
            {
                TotalItems = menucategories.Count(),
                Items = pagination
            };
            return queryResult;
        }
        public string GeneratePdfTemplateString(QueryResult<MenuCategoryDTO> menucategory)
        {
            var sb = new StringBuilder();

            sb.Append(@"<html>
                            <head>
                                <h1>Menu Category</h1>
                            </head>
                            <body>
                                <table align='center'>");
            sb.Append(@"<thead>
                            <tr>
                                        <th>Id</th>
                                        <th>Name</th>
                                        <th>Status</th>
                                    </tr></thead>");
            foreach (var item in menucategory.Items)
            {


                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                  </tr>", item.Id, item.Name, item.Status==1?"Active":"InActive");
            }

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
        public async Task Delete(int id)
        {
            var menuCategory = await _menuCategoryRepository.GetSingle(id);
            _menuCategoryRepository.Delete(menuCategory);
            await _unitOfWork.Commit();
        }
        public async Task<bool> ChangeStatus(int id)
        {
            try
            {
                MenuCategory obj = await _menuCategoryRepository.GetSingle(id);
                if (obj != null)
                {
                    if (obj.Status == (int)Enums.MenuCategoryStatus.Active)
                    {
                        obj.Status = (int)Enums.MenuCategoryStatus.Inactive;
                    }
                    else
                    {
                        obj.Status = (int)Enums.MenuCategoryStatus.Active;
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
        public async Task<IEnumerable<SelectListItem>> GetParent()
        {
            //where id!=id
            var menuCategory = _menuCategoryRepository.GetAll();
            var parent = new List<SelectListItem>();
            foreach (var item in menuCategory)
            {
                item.Name =await AppendParent(item.ParentId, item.Name);
                parent.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            //var parentCategory = _categoryRepository.GetAll().Select(x => new SelectListItem
            //{
            //    Text =AppendParent(x.ParentId,x.Name),
            //    Value = x.Id.ToString()
            //});
            return parent;
        }
        public async Task<string>  AppendParent(int? parentId, string menucategoryName)
        {
            try
            {
                if (parentId == null || parentId == 0)
                {
                    return menucategoryName;
                }

                var menucategory =await  _menuCategoryRepository.GetSingle((int)parentId);
                if (menucategory != null)
                {
                    menucategoryName = menucategory.Name + "->" + menucategoryName;
                    await AppendParent(menucategory.ParentId, menucategoryName);
                }
                return menucategoryName;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public void SaveChanges()
        {
            this._unitOfWork.Commit();
        }
    }
}
