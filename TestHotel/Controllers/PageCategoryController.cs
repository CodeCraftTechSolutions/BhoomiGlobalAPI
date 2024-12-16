using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Service.Infrastructure;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BhoomiGlobaAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageCategoryController : ControllerBase
    {
        private IPageCategoryService _pageCategoryService;
        readonly PageCategoryCodeSettings _pageCategoryCodeSettings;
        private readonly IConverter _converter;
        public PageCategoryController
        (
            IPageCategoryService pageCategoryService,
            IConverter converter,
            IOptionsSnapshot<PageCategoryCodeSettings> options
        )
        {
            _pageCategoryService = pageCategoryService;
            _pageCategoryCodeSettings = options.Value;
            _converter = converter;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _pageCategoryService.GetAll();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        //[HttpGet]
        //[Route("GetLLPageCategoryCode")]
        //public async Task<IActionResult> GetLLPageCategoryCode()
        //{
        //    try
        //    {
        //        //Enums enums = new Enums();
        //        //var result = enums.ExportEnum<LLPageCategoryCode>();
        //        var results = _pageCategoryCodeSettings.ToDynamic();
        //        var outputs=new List<KeyValueDTO>();
        //        foreach(var res in results)
        //        {
        //            var kv = new KeyValueDTO();
        //            kv.Key = res.Key;
        //            kv.Value = res.Value;
        //            outputs.Add(kv);
        //        }
        //        return Ok(outputs);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest();
        //    }
        //}
        [HttpGet]
        [Route("GetLLPageCategoryCodeRaw")]
        public async Task<IActionResult> GetLLPageCategoryCodeRaw()
        {
            try
            {
                var results = _pageCategoryCodeSettings;
                return Ok(results);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _pageCategoryService.GetPageCategoryById(id);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }


        [Route("PageCategoryItem")]
        [HttpPost]
        public async Task<IActionResult> PageCategoryItem(QueryObject query)
        {
            var result = await _pageCategoryService.PageCategoryList(query);
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
        //        var result = await _pageCategoryService.PageCategoryList(query);

        //        using (var workBook = new XLWorkbook())
        //        {
        //            var workSheet = workBook.Worksheets.Add("PageCategory");
        //            var currentRow = 1;

        //            workSheet.Cell(currentRow, 1).SetValue("ID");
        //            workSheet.Cell(currentRow, 2).SetValue("Name");
        //            workSheet.Cell(currentRow, 3).SetValue("Description");
                    


        //            foreach (var item in result.Items)
        //            {
                        
        //                currentRow++;
        //                workSheet.Cell(currentRow, 1).SetValue(item.Id);
        //                workSheet.Cell(currentRow, 2).SetValue(item.Name);
        //                workSheet.Cell(currentRow, 3).SetValue(item.Description);
        //            }

        //            using (var stream = new MemoryStream())
        //            {
        //                workBook.SaveAs(stream);
        //                stream.Seek(0, SeekOrigin.Begin);
        //                var content = stream.ToArray();
        //                return File(
        //                    content,
        //                     "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                    "PageCategory.xlsx"
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
                var result = await _pageCategoryService.PageCategoryList(query);
                return File(_converter.Convert(PrintPdfHelper.CreateTablePDF(_pageCategoryService.GeneratePdfTemplateString(result))), "application/pdf");
            }
            catch (Exception e)
            {
                throw;
            }

        }
        [HttpPost]
        [Route("AddPageCategory")]
        public async Task<IActionResult> AddPageCategory(PageCategoryDTO pagecategory)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    int result = 0;
                    if (pagecategory.Id == 0)
                    {
                        //if (this._pageCategoryService.IsExisting(pagecategory.CategoryCode))
                        //{
                        //    return BadRequest("Category Code Already Exists. Choose a New Code.");
                        //}
                        result = await _pageCategoryService.Create(pagecategory);
                    }
                    else
                    {
                        result = await _pageCategoryService.Update(pagecategory);
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
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("UpsertPageCategory")]
        public async Task<IActionResult> UpsertPageCategory(PageCategoryDTO pagecategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    int result = 0;
                    if (pagecategory.Id == 0)
                    {
                        result = await _pageCategoryService.Create(pagecategory);
                    }
                    else
                    {
                        result = await _pageCategoryService.Update(pagecategory);
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
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("DeletePageCategory/{id}")]
        public async Task<IActionResult> DeletePageCategory(int id)
        {
            try
            {
                if (id != 0)
                {
                    await _pageCategoryService.Delete(id);
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
    }
}
