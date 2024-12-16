using BhoomiGlobal.Service.Infrastructure;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.HelperClass;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BhoomiGlobaAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private IPageService _lLpageService;
        readonly PhotoSettings photoSettings;
        readonly PageCategoryCodeSettings pageCategoryCodeSettings;
        private readonly IConverter _converter;
        IWebHostEnvironment host;

        public PageController(
            IPageService lLpageService,
            IWebHostEnvironment environment,
            IOptionsSnapshot<PhotoSettings> options,
            IConverter converter,
            IOptionsSnapshot<PageCategoryCodeSettings> optionPage
            )
        {
            host = environment;
            photoSettings = options.Value;
            pageCategoryCodeSettings = optionPage.Value;
            _lLpageService = lLpageService;
            _converter = converter;
        }



        [HttpGet]
        [Route("GetAllForWeb")]
        public IActionResult GetAllForWeb()
        {
            try
            {
                var result = _lLpageService.GetAllForWeb();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpGet]
        [Route("GetByIdForWeb/{id}")]
        public IActionResult GetByIdForWeb(int id)
        {
            try
            {
                var result = _lLpageService.GetByIdForWeb(id);
                foreach(var img in result.PageImages)
                {
                    img.ImagePath = "UploadsllPageImage" + "//" + img.ImagePath;
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong while trying to get LLPage Detail.");
            }
        }


        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _lLpageService.GetAll();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }



        [HttpGet]
        [Route("getbyid/{id}")]
        //[DataPermissionAttribute(new Enums.DataPermissions[] { Enums.DataPermissions.Page_Edit })]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _lLpageService.GetPageById(id);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
        
        [HttpGet]
        [Route("getbyIdorCode")]
        public async Task<IActionResult> GetByIdorCode(int id = 0, string code = "")
        {
            try
            {
                if (id > 0)
                {
                    var result = await _lLpageService.GetPageById(id);
                    return Ok(result);
                }
                else if (!string.IsNullOrEmpty(code))
                {
                    var result = await _lLpageService.GetPageByCode(code);
                    return Ok(result);
                }
                else return BadRequest("Invalid.");

               
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }


        [HttpGet]
        [Route("GetLLpageBypageCategoryId/{pageCategoryId}")]
        public IActionResult GetLLpageBypageCategoryId(int pageCategoryId)
        {
            try
            {
                var result = _lLpageService.GetPageByPageCategoryId(pageCategoryId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [Route("LlPageItem")]
        [HttpPost]
        public async Task<IActionResult> LlPageItem(QueryObjectPage query)
        {
            var result = await _lLpageService.PageList(query);
            return Ok(result);
        }


        //[Route("ExportToExcel")]
        //[HttpPost]
        //public async Task<FileContentResult> ExportToExcel(QueryObjectPage query)
        //{
        //    try
        //    {

        //        if (query.printall)
        //        {
        //            query.Page = 1;
        //            query.PageSize = int.MaxValue;
        //        }
        //        var result = await _lLpageService.PageList(query);

        //        using (var workBook = new XLWorkbook())
        //        {
        //            var workSheet = workBook.Worksheets.Add("Page");
        //            var currentRow = 1;

        //            workSheet.Cell(currentRow, 1).SetValue("ID");
        //            workSheet.Cell(currentRow, 2).SetValue("Title ");
        //            workSheet.Cell(currentRow, 3).SetValue("ShortDescription ");
        //            workSheet.Cell(currentRow, 4).SetValue("LongDescription  ");
        //            workSheet.Cell(currentRow, 5).SetValue("PageCategoryName  ");
        //            workSheet.Cell(currentRow, 6).SetValue("PageName");

        //            foreach (var item in result.Items)
        //            {

        //                currentRow++;
        //                workSheet.Cell(currentRow, 1).SetValue(item.Id);
        //                workSheet.Cell(currentRow, 2).SetValue(item.Title);
        //                workSheet.Cell(currentRow, 3).SetValue(item.ShortDescription);
        //                workSheet.Cell(currentRow, 4).SetValue(item.LongDescription);
        //                workSheet.Cell(currentRow, 5).SetValue(item.PageCategoryName);
        //                workSheet.Cell(currentRow, 6).SetValue(item.PageName);
        //            }

        //            using (var stream = new MemoryStream())
        //            {
        //                workBook.SaveAs(stream);
        //                stream.Seek(0, SeekOrigin.Begin);
        //                var content = stream.ToArray();
        //                return File(
        //                    content,
        //                     "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                    "Page.xlsx"
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
        public async Task<FileContentResult> ExportToPdf(QueryObjectPage query)
        {
            try
            {

                if (query.printall)
                {
                    query.Page = 1;
                    query.PageSize = int.MaxValue;
                }
                var result = await _lLpageService.PageList(query);
                return File(_converter.Convert(PrintPdfHelper.CreateTablePDF(_lLpageService.GeneratePdfTemplateString(result))), "application/pdf");
            }
            catch (Exception e)
            {
                throw;
            }

        }


        [HttpPost]
        [Route("AddLlPage")]
        public async Task<IActionResult> AddLlPage(PageDTO masterlist)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int result = 0;
                    if (masterlist.Id == 0)
                    {
                        result = await _lLpageService.Create(masterlist);
                    }
                    else
                    {
                        result = await _lLpageService.Update(masterlist);
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
        [Route("UpsertLlPage")]
        public async Task<IActionResult> UpsertLlPage(PageDTO masterlist)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int result = 0;
                    if (masterlist.Id == 0)
                    {
                        result = await _lLpageService.Create(masterlist);
                    }
                    else
                    {
                        result = await _lLpageService.Update(masterlist);
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
        [Route("DeleteLlpage/{id}")]
        public async Task<IActionResult> DeleteLlpage(int id)
        {
            try
            {
                if (id != 0)
                {
                    await _lLpageService.Delete(id);
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


        [HttpPost]
        [Route("UploadImages/{id}")]
        public async Task<IActionResult> UploadImages(int id)
        {
            try
            {
                var files = Request.Form.Files;
                var llpage = await _lLpageService.GetPageById(id);
                if (llpage == null) return NotFound();

                if (files == null || !files.Any()) return BadRequest("No file found");
                foreach (var item in files)
                {
                    if (item.Length > photoSettings.MaxBytes) return BadRequest(" Max file size exceed.");
                    if (!photoSettings.AcceptedFileTypes.Any(x => x.ToLower() == Path.GetExtension(item.FileName.ToLower())))
                    {
                        return BadRequest("Invalid file type.");
                    }
                }
                string uploadFolderPath = Path.Combine(host.ContentRootPath, "UploadsllPageImage");
                string uploadImagePath = Path.Combine(host.ContentRootPath, "UploadsllPageImage");
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                uploadImagePath = Path.Combine(host.ContentRootPath, "UploadsllPageImage");
                if (!Directory.Exists(uploadImagePath))
                {
                    Directory.CreateDirectory(uploadImagePath);
                }
                var listfilename = new List<string>();
                foreach (var item in files)
                {
                    var filename = string.Empty;
                    if (item != null)
                    {
                        filename = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                        var filepath = Path.Combine(uploadImagePath, filename);
                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            await item.CopyToAsync(stream);
                            listfilename.Add(filename);
                        }
                    }
                }
                await _lLpageService.UploadImage(id, listfilename);

                var images = _lLpageService.GetPageImages(id).Select(x => new
                {
                    Id = x.Id,
                    url = "UploadsllPageImage" + "\\" + x.ImagePath
                });

                return Ok(images);
            }
            catch (Exception ex)
            {

                return BadRequest("Something went wrong");
            }

        }
        
        
        [HttpPost]
        [Route("UploadSectionImages")]
        public async Task<IActionResult> UploadSectionImages()
        {
            try
            {
                var files = Request.Form.Files;

                if (files == null || !files.Any()) return BadRequest("No file found");
                foreach (var item in files)
                {
                    if (item.Length > photoSettings.MaxBytes) return BadRequest(" Max file size exceed.");
                    if (!photoSettings.AcceptedFileTypes.Any(x => x.ToLower() == Path.GetExtension(item.FileName.ToLower())))
                    {
                        return BadRequest("Invalid file type.");
                    }
                }
                string uploadFolderPath = Path.Combine(host.ContentRootPath, "UploadsllPageSectionsImage");
                string uploadImagePath = Path.Combine(host.ContentRootPath, "UploadsllPageSectionsImage");
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                uploadImagePath = Path.Combine(host.ContentRootPath, "UploadsllPageSectionsImage");
                if (!Directory.Exists(uploadImagePath))
                {
                    Directory.CreateDirectory(uploadImagePath);
                }
                var listfilename = new List<string>();
                foreach (var item in files)
                {
                    var filename = string.Empty;
                    if (item != null)
                    {
                        filename = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                        var filepath = Path.Combine(uploadImagePath, filename);
                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            await item.CopyToAsync(stream);
                            listfilename.Add(filename);
                        }
                    }
                }

                var Datas = new
                {
                    ImageUrl = "UploadsllPageSectionsImage" + "\\" + listfilename.FirstOrDefault(),
                    FileName = files.FirstOrDefault().FileName,
                    
                };
                return Ok(Datas);
            }
            catch (Exception ex)
            {

                return BadRequest("Something went wrong");
            }

        }


        [HttpGet]
        [Route("GetllPageImage/{id}")]
        public IActionResult GetllPageImage(int id)
        {
            var images = _lLpageService.GetPageImages(id).Select(x => new
            {
                Id = x.Id,
                url = "UploadsllPageImage" + "\\" + x.ImagePath,
                IsPrimary = x.IsPrimary
            });

            return Ok(images);
        }
        [Route("PatchllPageImage")]
        [HttpPost]
        public async Task<IActionResult> PatchllPageImage(PageImageCheckPrimaryDTO model)
        {
            try
            {
                await this._lLpageService.PatchPageImage(model);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong while trying to patch Page Image Model.");
            }
        }
        [HttpPost]
        [Route("DeleteImage/{Id}")]
        public async Task<IActionResult> DeleteImage(int Id)
        {
            try
            {
                /*Preserve objectname in some value: */
                var llPageImage = await _lLpageService.GetPageImage(Id);
                int llpid = llPageImage.PageId;
                /*Delete from Database: */
                await _lLpageService.DeleteImage(Id);
                var uploadImagePath = Path.Combine(host.ContentRootPath, "UploadsllPageImage");
                var fileInfo = (uploadImagePath + "\\" + llPageImage.ImagePath);
                /*Delete from the Disk: */
                System.IO.File.Delete(fileInfo);
                /*Now return remaining images: */
                var remaining = _lLpageService.GetPageImages(llpid)
                    .Select(x => new {
                        Id = x.Id,
                        url = "UploadsllPageImage" + "\\" + x.ImagePath
                    });
                return Ok(remaining);
            }
            catch (Exception e)
            {
                return BadRequest("Failed to Delete the Image.");
            }
        }

        #region FAQ
        [HttpGet]
        [Route("FAQ")]
        public async Task<IActionResult> FAQ()
        {
            try
            {
                var result = await _lLpageService.GetFAQ();
                return Ok(result);

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        #endregion

        #region Support
        [HttpGet]
        [Route("Support")]
        public async Task<IActionResult> Support()
        {
            try
            {
                var result = await _lLpageService.Support();
                return Ok(result);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("AboutUs")]
        public async Task<IActionResult> AboutUs()
        {
            try
            {
                var result = await _lLpageService.AboutUs();
                return Ok(result);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region TermsAndCondition
        [HttpGet]
        [Route("TermsAndConditionWeb")]
        public async Task<IActionResult> TermsAndConditionWeb()
        {
            try
            {
                var result = await _lLpageService.TermsAndConditionsWeb();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        #endregion
    }
}
