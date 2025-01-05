using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static BhoomiGlobalAPI.HelperClass.Enums;

namespace BhoomiGlobaAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarouselController : ControllerBase
    {


        readonly ICarouselService _carouselService;
        readonly PhotoSettings photoSettings;
        IWebHostEnvironment host;

        public CarouselController(
            IWebHostEnvironment environment,
            IOptionsSnapshot<PhotoSettings> options,
            ICarouselService carouselService
            )
        {
            host = environment;
            photoSettings = options.Value;
            _carouselService = carouselService;
        }


        [HttpPost,Route("GetQueryCarousels")]
        public async Task<IActionResult> GetQueryCarousels(CarouselQueryObject query)
        {
            try
            {
                var carousels = await _carouselService.GetQueryCarousels(query);
                return Ok(carousels);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong while trying to get Carousels.");
            }
        }


        [HttpGet]
        [Route("GetCarouselEnum")]
        public object GetCarouselEnum()
        {
            try
            {
                Enums enums = new Enums();
                var result = enums.ExportEnum<CarouselType>();
                return result;
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("GetTargetModuleEnum")]
        public object GetTargetModuleEnum()
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


        [HttpPost]
        [Route("CreateCarousel")]
        public async Task<IActionResult> CreateCarousel(CarouselFewerItemsDTO carousel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    if (carousel.Id > 0)
                    {

                        var id=await _carouselService.Update(carousel);
                        return Ok(id);
                    }
                    else
                    {
                        var id = await _carouselService.Create(carousel);
                        return Ok(id);
                    }

                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return BadRequest();
        }


        [HttpPost]
        [Route("UpsertCarousel")]
        public async Task<IActionResult> UpsertCarousel(CarouselFewerItemsDTO carousel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (carousel.Id > 0)
                    {

                        var id = await _carouselService.Update(carousel);
                        return Ok(id);
                    }
                    else
                    {
                        var id = await _carouselService.Create(carousel);
                        return Ok(id);
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return BadRequest();
        }


        [Route("AllCarouselHome")]
        [HttpGet]
       // [DataPermissionAttribute(new Enums.DataPermissions[] { Enums.DataPermissions.Home_ViewAll })]
        public async Task<IActionResult> GetAllCarouselHome(int storeId, long brandId, int affiliateId, int homepageId)
        {
            var carousel = _carouselService.GetAll(storeId, brandId, affiliateId, homepageId);
            return Ok(carousel);
        }


        [Route("AllCarousel")]
        [HttpGet]
        public async Task<IActionResult> GetAllCarousel(int storeId, long brandId, int affiliateId, int homepageId)
        {
            var carousel = _carouselService.GetAll(storeId,brandId,affiliateId,homepageId);
            return Ok(carousel);
        }


        [Route("GetCarouselById/{carouselId}")]
        [HttpGet]
      //  [DataPermissionAttribute(new Enums.DataPermissions[] { Enums.DataPermissions.Home_Edit })]
        public async Task<IActionResult> GetCarouselById(int carouselId)
        {
            try
            {
                var carousel = await _carouselService.GetCarouselById(carouselId);
                return Ok(carousel);
            }
            catch(Exception e)
            {
                return BadRequest("Something went wrong while trying to get Carousel By Id.");
            }
        }

        [HttpPost]
        [Route("UploadImage/{id}")]
        public async Task<IActionResult> UploadImage(int id)
        {
            try
            {
                var files = Request.Form.Files;
                var carousel = _carouselService.GetCarouselImagePath(id);
                if (carousel == null) return NotFound();
                if (carousel.ImagePath != null)
                {
                    var _uploadImagePath = Path.Combine(host.ContentRootPath, "Uploads");
                    string imageName= _carouselService.GetImageName(id);
                    var fileInfo = (_uploadImagePath + "\\" + imageName);
                    System.IO.File.Delete(fileInfo);
                }

                if (files == null || !files.Any()) return BadRequest("No file found");
                //foreach (var item in files)
                //{
                //    //if (item.Length > photoSettings.MaxBytes) return BadRequest(" Max file size exceed.");
                //    if (!photoSettings.AcceptedFileTypes.Any(x => x.ToLower() == Path.GetExtension(item.FileName.ToLower())))
                //    {
                //        return BadRequest("Invalid file type.");
                //    }
                //}


                string uploadFolderPath = Path.Combine(host.ContentRootPath, "Uploads");
                string uploadImagePath = Path.Combine(host.ContentRootPath, "Uploads");
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                uploadImagePath = Path.Combine(host.ContentRootPath, "Uploads");
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
                await _carouselService.UploadImage(id, listfilename.FirstOrDefault());
                var image = _carouselService.GetCarouselImage(id);
                return Ok(image);
            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong");
            }

        }
        
        
        [HttpPost]
        [Route("UploadHomeBannerSmallImage/{id}")]
        public async Task<IActionResult> UploadHomeBannerSmallImage(int id)
        {
            try
            {
                var files = Request.Form.Files;
                var carousel = _carouselService.GetCarouselHomeBannerSmallImagePath(id);
                if (carousel == null) return NotFound();
                if (carousel.ImagePath != null)
                {
                    var _uploadImagePath = Path.Combine(host.ContentRootPath, "Uploads");
                    string imageName= _carouselService.GetHomeBannerSmallImageName(id);
                    var fileInfo = (_uploadImagePath + "\\" + imageName);
                    System.IO.File.Delete(fileInfo);
                }

                if (files == null || !files.Any()) return BadRequest("No file found");
                foreach (var item in files)
                {
                    if (item.Length > photoSettings.MaxBytes) return BadRequest(" Max file size exceed.");
                    if (!photoSettings.AcceptedFileTypes.Any(x => x.ToLower() == Path.GetExtension(item.FileName.ToLower())))
                    {
                        return BadRequest("Invalid file type.");
                    }
                }


                string uploadFolderPath = Path.Combine(host.ContentRootPath, "Uploads");
                string uploadImagePath = Path.Combine(host.ContentRootPath, "Uploads");
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                uploadImagePath = Path.Combine(host.ContentRootPath, "Uploads");
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
                await _carouselService.UploadHomeBannerSmall(id, listfilename.FirstOrDefault());
                var image = _carouselService.GetCarouselHomeBannerSmallImage(id);
                return Ok(image);
            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong");
            }

        }

        [HttpGet]
        [Route("GetCarouselImage/{id}")]
        public IActionResult GetCarouselImage(int id)
        {
            var image = _carouselService.GetCarouselImage(id);
            return Ok(image);
        }

        [HttpGet]
        [Route("GetHomeBannerImage/{id}")]
        public IActionResult GetHomeBannerImage(int id)
        {
            var image = _carouselService.GetCarouselHomeBannerSmallImage(id);
            return Ok(image);
        }

        [HttpPost]
        [Route("DeleteCarousel/{carouselId}")]
        //[DataPermissionAttribute(new Enums.DataPermissions[] { Enums.DataPermissions.Home_Delete })]
        public async Task<IActionResult> DeleteImage(int carouselId)
        {
            try
            {
                /*Preserve objectname in some value: */
                var carouselImage = _carouselService.GetCarouselImagePath(carouselId);
                /*Delete from Database: */
                await _carouselService.Delete(carouselId);
                var uploadImagePath = Path.Combine(host.ContentRootPath, "Uploads");
                var fileInfo = (uploadImagePath + "\\" + carouselImage.ImagePath);
                /*Delete from Disk: */
                System.IO.File.Delete(fileInfo);
                return Ok(new { Success=true});
            }
            catch (Exception e)
            {
                return BadRequest("Failed to Delete the Image.");
            }
        }

        [HttpPost]
        [Route("PatchOrder")]/*Patch OrderBy as manually reordered by Admin User*/
        public async Task<IActionResult> PatchOrder(List<PatchOrderDTO> patchOrders)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var success = await _carouselService.PatchOrders(patchOrders);
                    return Ok(new { Success = success });
                }
                catch (Exception e)
                {
                    return BadRequest("Something went wrong while trying to patch Orders in Carousel.");
                }
            }
            else
            {
                return BadRequest("ModelState not valid.");
            }
        }

        //GetEntityByType
        #region mobile-navigator
        [HttpGet]
        [Route("GetEntitiesByType/{enumValue}")]
        public async Task<IActionResult> GetEntitiesByType(int enumValue)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    switch (enumValue)
                    {
                        case (int)Enums.TargetModule.Page:
                            return Ok("TO DO");
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
        [HttpPost]
        [Route("PostStoreMobileParams")]
        public async Task<IActionResult> PostStoreMobileParams(CarouselMobileDTO carmob)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (carmob.Id > 0)
                    {
                        await _carouselService.PatchCarousel(carmob);
                        return Ok(new { Success = true });
                    }

                }
            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong when trying to patch Carousel.");
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("ArrangeOrder")]
        public async Task<IActionResult> ArrangeOrder(List<SliderOrder> SliderOrder)
        {
            try
            {
                await _carouselService.ArrangeOrder(SliderOrder);
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Front End
        [Route("SlidersByType")]
        [HttpGet]
        public async Task<IActionResult> SlidersByType(int sliderType)
        {
            var carousel = _carouselService.GetAllByType(sliderType);
            return Ok(carousel);
        }
        #endregion Front End

  
    }
}