using BhoomiGlobalAPI.Api.Controllers;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Service.Infrastructure;
using BhoomiGlobalAPI.Service.IService;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BhoomiGlobaAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuCategoryController : BaseApiController
    {
        private readonly IMenuCategoryService _menuCategoryService;
        private readonly IUserDetailsService _userDetailService;
        private readonly IConverter _converter;
        public MenuCategoryController(IMenuCategoryService menuCategoryServie, IUserDetailsService userDetailService,IConverter converter)
        {
            _menuCategoryService = menuCategoryServie;
            _userDetailService = userDetailService;
            _converter = converter;
        }


        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _menuCategoryService.GetAll();
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
                var result = await _menuCategoryService.GetMenuCategoryById(id);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }



        [Route("MenuCategoryItem")]
        [HttpPost]
        public async Task<IActionResult> MenuCategoryItem(QueryObject query)
        {
            var result = await _menuCategoryService.MenuCategoryList(query);
            return Ok(result);
        }



        //[Route("ExportToExcel")]
        //[HttpPost]
        //public async Task<FileContentResult> ExportToExcel(QueryObject query)
        //{
        //    try
        //    {

        //        if (query.printall)
        //        {
        //            query.Page = 1;
        //            query.PageSize = int.MaxValue;
        //        }
        //        var result = await _menuCategoryService.MenuCategoryList(query);

        //        using (var workBook = new XLWorkbook())
        //        {
        //            var workSheet = workBook.Worksheets.Add("PageCategory");
        //            var currentRow = 1;

        //            workSheet.Cell(currentRow, 1).SetValue("ID");
        //            workSheet.Cell(currentRow, 2).SetValue("Name");
        //            workSheet.Cell(currentRow, 3).SetValue("Status");



        //            foreach (var item in result.Items)
        //            {

        //                currentRow++;
        //                workSheet.Cell(currentRow, 1).SetValue(item.Id);
        //                workSheet.Cell(currentRow, 2).SetValue(item.Name);
        //                workSheet.Cell(currentRow, 3).SetValue(item.Status==1?"Active":"InActive");
        //            }

        //            using (var stream = new MemoryStream())
        //            {
        //                workBook.SaveAs(stream);
        //                stream.Seek(0, SeekOrigin.Begin);
        //                var content = stream.ToArray();
        //                return File(
        //                    content,
        //                     "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                    "MenuCategory.xlsx"
        //                 );

        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }

        //}



        [Route("ExportToPdf")]
        [HttpPost]
        public async Task<FileContentResult> ExportToPdf(QueryObject query)
        {
            try
            {

                if (query.printall)
                {
                    query.Page = 1;
                    query.PageSize = int.MaxValue;
                }
                var result = await _menuCategoryService.MenuCategoryList(query);
                return File(_converter.Convert(PrintPdfHelper.CreateTablePDF(_menuCategoryService.GeneratePdfTemplateString(result))), "application/pdf");
            }
            catch (Exception e)
            {
                throw;
            }

        }



        [HttpPost]
        [Route("CreateMenuCategory")]
        public async Task<IActionResult> CreateMenuCategory(MenuCategoryDTO masterlist)
        {
            Int64 UserId = await _userDetailService.GetUserDetailId(GetUserId());
            try
            {
                if (ModelState.IsValid)
                {
                    int result = 0;
                    if (masterlist.Id == 0)
                    {

                        result = await _menuCategoryService.Create(masterlist, UserId);
                    }
                    else
                    {
                        result = await _menuCategoryService.Update(masterlist, UserId);
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
        [Route("UpsertMenuCategory")]
        public async Task<IActionResult> UpsertMenuCategory(MenuCategoryDTO masterlist)
        {
            Int64 UserId = await _userDetailService.GetUserDetailId(GetUserId());
            try
            {
                if (ModelState.IsValid)
                {
                    int result = 0;
                    if (masterlist.Id == 0)
                    {
                          
                        result = await _menuCategoryService.Create(masterlist,UserId);
                    }
                    else
                    {
                        result = await _menuCategoryService.Update(masterlist,UserId);
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
        [Route("DeleteMenuCategory/{id}")]
        public async Task<IActionResult> DeleteMenuCategory(int id)
        {
            try
            {
                if (id != 0)
                {
                    await _menuCategoryService.Delete(id);
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
        [Route("ChangeMenuCategoryStatus/{id}")]
        public async Task<IActionResult> ChangeMenuCategoryStatus(int Id)
        {
            try
            {
                //Int64 userDetailId = await _userDetailService.GetUserDetailId(GetUserId());
                var result = await _menuCategoryService.ChangeStatus(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Route("Parents/{id}")]
        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> Parent(int id=0)
        {
            try
            {
                var result = await _menuCategoryService.GetParent();
                return result.Where(a => a.Value != id.ToString());
                //return result;
            }
            catch(Exception e)
            {
                throw;
            }
           
        }


        //[Authorize]
        [Route("menuCategory")]
        [HttpGet]
        public IActionResult menuCategory()
        {
            //string userId = GetUserName();
            var menuCategory = _menuCategoryService.menuCategory;
            foreach (var item in menuCategory)
            {
                if (item.Status == 1)
                    item.IsActive = "Active";
                else
                    item.IsActive = "InActive";
                if (item.Icon != null && item.Icon.Trim() != string.Empty)
                {
                    item.Icon = "Uploads\\" + item.Icon;
                }
            }
            return Ok(menuCategory);
        }

    }
}
