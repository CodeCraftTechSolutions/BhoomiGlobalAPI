using BhoomiGlobalAPI.Api.Controllers;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Repository.IRepository;
using BhoomiGlobalAPI.Service.Infrastructure;
using BhoomiGlobalAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using static BhoomiGlobalAPI.HelperClass.Enums;


namespace BhoomiGlobaAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : BaseApiController
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IUserDetailsService _userDetailService;
        private readonly IPageCategoryService _pageCategoryService;
        private readonly IPageRepository _lLpageRepository;

        public MenuItemController(IMenuItemService menuItemService, IUserDetailsService userDetailService
                                ,IPageCategoryService pageCategoryService, IPageRepository lLpageRepository)
        {
            _menuItemService = menuItemService;
            _userDetailService = userDetailService;
            _pageCategoryService = pageCategoryService;
            _lLpageRepository = lLpageRepository;
        }



        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _menuItemService.GetAll();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }



        [HttpGet]
        [Route("GetMenuItemByMenuCategoryId/{Id}")]
        public async Task<IActionResult> GetMenuItemByMenuCategoryId(int Id)
        {
            try
            {
                var result =await  _menuItemService.GetMenuItemByMenuCategoryId(Id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }



        [HttpGet]
        [Route("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _menuItemService.GetMenuItemById(id);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }



        [Route("MenuItem")]
        [HttpPost]
        public async Task<IActionResult> MenuItem(QueryObject query)
        {
            var result = await _menuItemService.MenuItemList(query);
            return Ok(result);
        }




        [HttpPost]
        [Route("UpsertMenuItem")]
        public async Task<IActionResult> UpsertMenuItem(MenuItemDTO masterlist)
        {
            Int64 UserId = await _userDetailService.GetUserDetailId(GetUserId());
            try
            {
                if (ModelState.IsValid)
                {
                    var entityData = _lLpageRepository.GetById(masterlist.EntityId);
                    //if (masterlist.MenuTypeId == (int)TargetModule.Store)
                    //{
                    //    masterlist.Url = "store/" + masterlist.EntityId;
                    //    if(entityData != null && !string.IsNullOrEmpty(entityData.PageUrlCode)) masterlist.CodeUrl = "store/" + entityData.PageUrlCode;
                    //}
                    //if (masterlist.MenuTypeId == (int)TargetModule.ProductCategory)
                    //{
                    //    masterlist.Url = "category/category-details/" + masterlist.EntityId;
                    //    if(entityData != null && !string.IsNullOrEmpty(entityData.PageUrlCode)) masterlist.CodeUrl = "category/category-details/" + entityData.PageUrlCode;
                    //}
                    //if (masterlist.MenuTypeId == (int)TargetModule.Product)
                    //{
                    //    masterlist.Url = "product/product-details/" + masterlist.EntityId;
                    //    if (entityData != null && !string.IsNullOrEmpty(entityData.PageUrlCode)) masterlist.CodeUrl = "product/product-details/" + entityData.PageUrlCode;
                    //}
                    if (masterlist.MenuTypeId == (int)TargetModule.Page)
                    {
                        masterlist.Url = "page/" + masterlist.EntityId;
                        if (entityData != null && !string.IsNullOrEmpty(entityData.PageUrlCode)) masterlist.CodeUrl = "page/" + entityData.PageUrlCode;
                    }   
                    //if (masterlist.MenuTypeId == (int)TargetModule.Brand)
                    //{
                    //    masterlist.Url = "brand/" + masterlist.EntityId;
                    //    if (entityData != null && !string.IsNullOrEmpty(entityData.PageUrlCode)) masterlist.CodeUrl = "brand/" + entityData.PageUrlCode;
                    //}

                    int result = 0;
                    if (masterlist.Id == 0)
                    {
                       
                        result = await _menuItemService.Create(masterlist, UserId);
                    }
                    else
                    {
                        result = await _menuItemService.Update(masterlist, UserId);
                    }
                    return Ok(result);
                }
                else
                {
                    return BadRequest("ModelState is not valid.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e); ;
            }
        }



        [HttpPost]
        [Route("DeleteMenuItem/{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            try
            {
                if (id != 0)
                {
                    await _menuItemService.Delete(id);
                    return Ok();
                }
                else
                {
                    return BadRequest("Not Found.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }




        [HttpGet]
        [Route("ChangeMenuItemStatus/{id}")]
        public async Task<IActionResult> ChangeMenuItemStatus(int Id)
        {
            try
            {
                //Int64 userDetailId = await _userDetailService.GetUserDetailId(GetUserId());
                var result = await _menuItemService.ChangeStatus(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpGet]
        [Route("GetMenuTypeModuleEnum")]
        public object GetMenuTypeModuleEnum()
        {
            try
            {
                Enums enums = new Enums();
                var result = enums.ExportEnum<TargetModule>();
                return result;
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }



        [Route("MenytypeItem")]
        [HttpPost]
        public async Task<IActionResult> MenytypeItem(QueryObject query)
        {
            var result = await _menuItemService.MenuItemList(query);
            return Ok(result);
        }



        [HttpGet]
        [Route("GetEntitiesByType/{enumValue}")]
        public IActionResult GetEntitiesByType(int enumValue)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    switch (enumValue)
                    {
                        case (int)Enums.TargetModule.Page:
                            var pageCategories = _pageCategoryService.GetAll();
                            return Ok(pageCategories);
                        default:
                            return BadRequest("NOT FOUND");
                    }
                }
                catch (Exception e)
                {
                    return BadRequest("Something went wrong while trying to Get Entities.");
                }
            }
            else
            {
                return BadRequest("ModelState not valid.");
            }
        }

        [HttpGet]
        [Route("FooterMenu")]
        public async Task<IActionResult> FooterMenu()
        {
            try
            {
               var result=await _menuItemService.FooterMenu();
               return Ok(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        } 

        [HttpGet]
        [Route("GetDefault")]
        public async Task<IActionResult> GetDefault()
        {
            try
            {
               var result=await _menuItemService.GetDefaultData();
               return Ok(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        } 



        [HttpGet]
        [Route("HeaderMenuNew")]
        public async Task<IActionResult> HederMenu1()
        {
            try
            {
               var result=await _menuItemService.HeaderMenuNew();
               return Ok(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        [HttpGet]
        [Route("HeaderMenu")]
        public async Task<IActionResult> HeaderMenu()
        {
            try
            {
                var result = await _menuItemService.HeaderMenu();
                if(result!=null )
                {
                    if (!string.IsNullOrEmpty(result.AppleStoreImageUrl))
                    {
                        result.AppleStoreImageUrl = "UploadsPlayStoreImage/" + result.AppleStoreImageUrl;
                    }
                    if (!string.IsNullOrEmpty(result.PlayStoreImageUrl))
                    {
                        result.PlayStoreImageUrl = "UploadsPlayStoreImage/" + result.PlayStoreImageUrl;
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
