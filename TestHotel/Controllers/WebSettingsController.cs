using BhoomiGlobal.Service.Infrastructure;
using BhoomiGlobalAPI.Api.Controllers;
using BhoomiGlobalAPI.DTO;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Service.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BhoomiGlobaAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebSettingsController : BaseApiController
    {
        private readonly IWebSettingsService _webSettingsService;
        private readonly IMenuCategoryService _menuCategoryService;
        private readonly IPageCategoryService _pageCategoryService;
        private readonly IPageService  _lLpageService;
        readonly PhotoSettings photoSettings;
        IWebHostEnvironment host;
        public WebSettingsController(IWebSettingsService webSettingsService,IMenuCategoryService menuCategoryService, IWebHostEnvironment environment, IPageCategoryService pageCategoryService,IPageService lLpageService, IOptionsSnapshot<PhotoSettings> options)
        {
            _webSettingsService = webSettingsService;
            _menuCategoryService = menuCategoryService;
            _pageCategoryService = pageCategoryService;
            _lLpageService = lLpageService;
            photoSettings = options.Value;
            host = environment;
        }


        [HttpGet]
        [Route("GetAllPageCategory")]
        public IActionResult GetAllPageCategory()
        {
            try
            {
                var result = _pageCategoryService.GetAll();
                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpGet]
        [Route("GetAllMenuCategory")]
        public IActionResult GetAllMenuCategory()
        {
            try
            {
                var result = _menuCategoryService.GetAll();
                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpGet]
        [Route("GetAllPages")]
        public IActionResult GetAllPages()
        {
            try
            {
                var result = _lLpageService.GetAll();
                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpPost]
        [Route("UpsertWebSettings")]
        public async Task<IActionResult> UpsertWebSettings(WebSettingsDTO masterlist)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int result = 0;
                    if (!string.IsNullOrEmpty(masterlist.AppleStoreImageUrl))
                    {
                        masterlist.AppleStoreImageUrl = masterlist.AppleStoreImageUrl.Replace("UploadsPlayStoreImage/", "");
                    }
                    if (!string.IsNullOrEmpty(masterlist.PlayStoreImageUrl))
                    {
                        masterlist.PlayStoreImageUrl = masterlist.PlayStoreImageUrl.Replace("UploadsPlayStoreImage/", "");
                    }
                    if (!string.IsNullOrEmpty(masterlist.SiteLogoImageUrl))
                    {
                        masterlist.SiteLogoImageUrl = masterlist.SiteLogoImageUrl.Replace("UploadsPlayStoreImage/", "");
                    }

                    if (masterlist.Id == 0)
                        result = await _webSettingsService.Create(masterlist);
                    else
                        result = await _webSettingsService.Update(masterlist);
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


        [HttpGet]
        [Route("GetFirstoRdefalut")]
        public object GetFirstoRdefalut()
        {
            try
            {
                var result = _webSettingsService.GetFirstOrDefault();
                if (!string.IsNullOrEmpty(result.AppleStoreImageUrl))
                {
                    result.AppleStoreImageUrl = "UploadsPlayStoreImage/" + result.AppleStoreImageUrl;
                }
                if (!string.IsNullOrEmpty(result.PlayStoreImageUrl))
                {
                    result.PlayStoreImageUrl = "UploadsPlayStoreImage/" + result.PlayStoreImageUrl;
                }
                if (!string.IsNullOrEmpty(result.SiteLogoImageUrl))
                {
                    result.SiteLogoImageUrl = "UploadsPlayStoreImage/" + result.SiteLogoImageUrl;
                }
                if (!string.IsNullOrEmpty(result.SiteFaviconImageUrl))
                {
                    result.SiteFaviconImageUrl = "UploadsPlayStoreImage/" + result.SiteFaviconImageUrl;
                }
                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        [HttpPost]
        [Route("UploadPlayStoreImage")]
        public async Task<IActionResult> UploadPlayStoreImage()
        {
            try
            {
                var files = Request.Form.Files;
                var websettings =  _webSettingsService.GetFirstOrDefault();
                if (websettings == null) return NotFound();

                if (files == null || !files.Any()) return BadRequest("No file found");
                foreach (var item in files)
                {
                    if (item.Length > photoSettings.MaxBytes) return BadRequest(" Max file size exceed.");
                    if (!photoSettings.AcceptedFileTypes.Any(x => x.ToLower() == Path.GetExtension(item.FileName.ToLower())))
                    {
                        return BadRequest("Invalid file type.");
                    }
                }
                string uploadFolderPath = Path.Combine(host.ContentRootPath, "UploadsPlayStoreImage");
                string uploadImagePath = Path.Combine(host.ContentRootPath, "UploadsPlayStoreImage");
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                uploadImagePath = Path.Combine(host.ContentRootPath, "UploadsPlayStoreImage");
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
                //await _webSettingsService.UploadImagePlayStoreImage(listfilename.FirstOrDefault());

                var image = new
                {
                    storeImageUrl = "UploadsPlayStoreImage/" + listfilename.FirstOrDefault()
                };

                return Ok(image);
            }
            catch (Exception ex)
            {

                return BadRequest("Something went wrong");
            }

        }


        [HttpGet]
        [Route("GetPlayStoreImage")]
        public IActionResult GetPlayStoreImage()
        {
            var image = _webSettingsService.GetPlayStoreImage();
            return Ok(image);
        }


        [HttpGet]
        [Route("GetSiteLogoImage")]
        public IActionResult GetSiteLogoImage()
        {
            var image = _webSettingsService.GetSiteLogoImage();
            return Ok(image);
        }

        [HttpGet]
        [Route("GetSiteFaviconImage")]
        public IActionResult GetSiteFaviconImage()
        {
            var image = _webSettingsService.GetSiteFaviconImage();
            return Ok(image);
        }
    }
}
