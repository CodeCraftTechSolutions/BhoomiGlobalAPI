using AutoMapper;
using BhoomiGlobal.Service.Extension;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Repository.Infrastructure;
using BhoomiGlobalAPI.Repository.IRepository;
using BhoomiGlobalAPI.Service.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace LogicLync.Service
{
    public class MenuItemService:IMenuItemService
    {
        IMenuItemRepository _menuItemRepository;
        IMapper _mapper;
        IPageCategoryRepository _pageCategoryRepository;
        IPageRepository _lLpageRepository;
        IMenuCategoryRepository _menuCategoryRepository;
        IWebSettingsRepository _webSettingRepository;
        IUnitOfWork _unitOfWork;
        
        public MenuItemService(IMenuItemRepository menuItemRepository,
                            IMenuCategoryRepository menuCategoryRepository, 
                            IMapper mapper,
                            IUnitOfWork unitOfWork,
                            IPageCategoryRepository pageCategoryRepository,
                            IPageRepository lLpageRepository, IWebSettingsRepository webSettingRepository)
        {
            _menuItemRepository = menuItemRepository;
            _mapper = mapper;
            _pageCategoryRepository = pageCategoryRepository;
            //_categoryRepository = categoryRepository;
            _lLpageRepository = lLpageRepository;
            _menuCategoryRepository = menuCategoryRepository;
            _webSettingRepository = webSettingRepository;
            _unitOfWork = unitOfWork; 

        }
        public IEnumerable<MenuItemDTO> GetAll()
        {
            IEnumerable<MenuItem> mneuItems = _menuItemRepository.GetAll();
            return _mapper.Map<IEnumerable<MenuItemDTO>>(mneuItems);
        }
        public async Task<int> Create(MenuItemDTO model, Int64 userId)
        {

            MenuItem obj = _mapper.Map<MenuItem>(model);
            obj.CreatedById = userId;
            obj.CreatedOn = DateTime.Now;
            obj.ModifiedById = userId;
            obj.ModifiedOn = DateTime.Now;
            await _menuItemRepository.Add(obj);
            await _unitOfWork.Commit();
            return obj.Id;
        }
        public async Task<int> Update(MenuItemDTO model, Int64 userId)
        {

            var menuItem = await _menuItemRepository.GetSingle(model.Id);
            if (menuItem != null)
            {
                menuItem.Id = model.Id;
                menuItem.MenuCategoryId = model.MenuCategoryId;
                menuItem.MenuTypeId = model.MenuTypeId;
                menuItem.EntityCategoryId = model.EntityCategoryId;
                menuItem.EntityId = model.EntityId;
                menuItem.Url = model.Url;
                menuItem.Status = model.Status;
                menuItem.ModifiedById = model.ModifiedById;
                menuItem.ModifiedOn = menuItem.ModifiedOn;

                await _unitOfWork.Commit();
            }
            return menuItem.Id;
        }
        public async Task<MenuItemDTO> GetMenuItemById(int id)
        {
            var menuItem = await _menuItemRepository.GetSingle(id);
            if (menuItem == null) return new MenuItemDTO();
            return _mapper.Map<MenuItemDTO>(menuItem);
        }
        public MenuItemDTO GetMenuItemById(int Id, bool isAuthorized)
        {
            throw new NotImplementedException();
        }
        public async Task<List<MenuItemDTO>> GetMenuItemByMenuCategoryId(int mcId)
        {
            
            List<MenuItem> menuItems = _menuItemRepository.GetAll(a => a.MenuCategoryId == mcId).ToList();
            var result= _mapper.Map<List<MenuItemDTO>>(menuItems);
            foreach (var items in result)
            {
                var enumDisplayStatus = (Enums.TargetModule)items.MenuTypeId;
                items.MenuTypeName = enumDisplayStatus.ToString();
                if (items.MenuTypeId == (int)Enums.TargetModule.Page)
                {
                    var pageCategory = await _pageCategoryRepository.GetSingle(items.EntityCategoryId);
                    if (pageCategory != null)
                    {
                        items.EntityCategoryName = pageCategory.Name;
                    }
                    var page = await _lLpageRepository.GetSingle(items.EntityId);
                    if (page != null)
                    {
                        items.EntityName = page.Title;
                    }
                    
                }
            }
            return result;
            
        }
        public MenuItemDTO GetMenuItemById(int Id, Expression<Func<MenuItem, bool>> where = null, params Expression<Func<MenuItem, object>>[] includeExpressions)
        {
            throw new NotImplementedException();
        }
         public async Task<QueryResult<MenuItemDTO>> MenuItemList(QueryObject query)
         {
            if (string.IsNullOrEmpty(query.SortBy))
            {
                query.SortBy = "Name";
            }
            var colomnMap = new Dictionary<string, Expression<Func<MenuItem, object>>>()
            {
                ["Id"] = p => p.Id,
                ["Status"] = p => p.Status,
                ["CreatedById"] =p=>p.CreatedById,
                ["CreatedOn"] =p=>p.CreatedOn,
                ["ModifiedById"] =p=>p.ModifiedById,
                ["ModifiedOn"] =p=>p.ModifiedOn

            };

            var menuItems= _menuItemRepository.GetAll();

            var result = await menuItems.ApplyOrdering(query, colomnMap).ToListAsync();
            int filterdatacount = menuItems.Count();

            var pagination = _mapper.Map<List<MenuItemDTO>>(result);

            var queryResult = new QueryResult<MenuItemDTO>
            {
                TotalItems = menuItems.Count(),
                Items = pagination
            };
            return queryResult;
        }
        public async Task Delete(int id)
        {
            var menuItem = await _menuItemRepository.GetSingle(id);
            _menuItemRepository.Delete(menuItem);
            await _unitOfWork.Commit();
        }
        public async Task<bool> ChangeStatus(int id)
        {
            try
            {
                MenuItem obj = await _menuItemRepository.GetSingle(id);
                if (obj != null)
                {
                    if (obj.Status == (int)Enums.MenuItemStatus.Active)
                    {
                        obj.Status = (int)Enums.MenuItemStatus.Inactive;
                    }
                    else
                    {
                        obj.Status = (int)Enums.MenuItemStatus.Active;
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

        #region FooterMenu
        public async Task<List<FooterMenuDTO>> FooterMenu()
        {
            var webSetting = await _webSettingRepository.All.FirstOrDefaultAsync();
            if (webSetting == null) return new List<FooterMenuDTO>();
            var menu = await _menuCategoryRepository
                        .AllIncluding(x => x.MenuItems)
                        .Where(x => x.ParentId == webSetting.FooterMenuCategoryId).ToListAsync();
            if (!menu.Any()) new List<FooterMenuDTO>();
            var footermenulist = new List<FooterMenuDTO>
            { 
                //new FooterMenuDTO
                //{
                //    AdditionData=new AdditionFooterInformation
                //    { 
                //        AboutUs= webSetting.AboutUs??string.Empty,
                //        Address= webSetting.AddressDetail??string.Empty,
                //        FacebookUrl =  webSetting.FacebookUrl?? string.Empty,
                //        InstagramUrl = webSetting.InstagramUrl?? string.Empty,
                //        TwitterUrl = webSetting.TwitterUrl?? string.Empty,
                //    } 
                //}
            };
           
            foreach (var item in menu)
            {
                var footer = new FooterMenuDTO
                {
                    Name = item.Name,
                    FooterMenu = new List<FooterMenuDTO>(),
                    IsParent = true,
                };

                await AddChildren(item, footer);
                footermenulist.Add(footer);
            }
            return footermenulist;
        }

        public async Task<List<HeaderMenuDTO>> HeaderMenuNew()
        {
            var webSetting = await _webSettingRepository.All.FirstOrDefaultAsync();
            if (webSetting == null) return new List<HeaderMenuDTO>();
            var menu = await _menuCategoryRepository
                        .AllIncluding(x => x.MenuItems)
                        .Where(x => x.ParentId == webSetting.HeaderMenuCategoryId).ToListAsync();
            if (!menu.Any()) new List<HeaderMenuDTO>();
            var headermenulist = new List<HeaderMenuDTO>
            {
                //new FooterMenuDTO
                //{
                //    AdditionData=new AdditionFooterInformation
                //    { 
                //        AboutUs= webSetting.AboutUs??string.Empty,
                //        Address= webSetting.AddressDetail??string.Empty,
                //        FacebookUrl =  webSetting.FacebookUrl?? string.Empty,
                //        InstagramUrl = webSetting.InstagramUrl?? string.Empty,
                //        TwitterUrl = webSetting.TwitterUrl?? string.Empty,
                //    } 
                //}
            };

            foreach (var item in menu)
            {
                var header = new HeaderMenuDTO
                {
                    Name = item.Name,
                    HeaderMenu = new List<HeaderMenuDTO>(),
                    IsParent = true,
                };

                await AddChildrenHeader(item, header);
                headermenulist.Add(header);
            }
            return headermenulist;
        }

        private async Task AddChildrenHeader(MenuCategory item, HeaderMenuDTO header)
        {

            foreach (var llpage in item.MenuItems)
            {
                var child = new HeaderMenuDTO();
                
                
                if (llpage.MenuTypeId == (int)Enums.TargetModule.Page)
                {
                    var page = await _lLpageRepository.GetSingle(llpage.EntityId);
                    if (page != null)
                    {
                        child.Name = page.Title;
                        child.CodeUrl = "/page/" + page.PageUrlCode;
                    }

                }
          
                child.Url = llpage.Url;
                header.HeaderMenu.Add(child);
            }
        }

        private async Task AddChildren(MenuCategory item, FooterMenuDTO footer)
        {
            
            foreach (var llpage in item.MenuItems)
            {
                var child = new FooterMenuDTO();
               
                
                if (llpage.MenuTypeId == (int)Enums.TargetModule.Page)
                {
                    var page = await _lLpageRepository.GetSingle(llpage.EntityId);
                    if (page != null)
                    {
                        child.Name = page.Title;
                        child.CodeUrl = "/page/" +  page.PageUrlCode;
                    }

                }
                
                child.Url = llpage.Url;
                footer.FooterMenu.Add(child);
            }

            



        }
        
        
        #endregion

        #region HeaderMenu
        public async Task<WebSettings> HeaderMenu()
        {
            var webSetting = await _webSettingRepository.All.FirstOrDefaultAsync();
            if (webSetting != null) return webSetting;
            return new WebSettings();
        }
        #endregion
        public void SaveChanges()
        {
            this._unitOfWork.Commit();
        }


    }
}
