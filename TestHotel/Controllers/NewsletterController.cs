using BhoomiGlobalAPI.Api.Controllers;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Repository.IRepository;
using BhoomiGlobalAPI.Service.IService;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogicLync.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsletterController : BaseApiController
    {
        private readonly INewsletterSubscriberService _newsletterSubscriberService;
        private readonly INewsletterService _newsletterService;
        private readonly IUserDetailsService _userDetailService;
        private readonly IEmailQueueService _emailQueueService;
        //private readonly IRemoteAPIService _remoteAPIService;
        //private readonly RemoteAPISetting remoteAPISettings;
        private readonly IConverter _converter;
        private readonly IUserDetailsRepository _userDetailsRepository;

        public NewsletterController(INewsletterSubscriberService newsletterSubscriberService, 
            IUserDetailsService userDetailService, 
            INewsletterService newsletterService,
            IEmailQueueService emailQueueService,
            //IRemoteAPIService remoteAPIService,
            IConverter  converter,
            IUserDetailsRepository userDetailsRepository
            //IOptionsSnapshot<RemoteAPISetting> optionsRemoteAPI
            )
        {
            _newsletterSubscriberService = newsletterSubscriberService;
            _userDetailService = userDetailService;
            _newsletterService = newsletterService;
            _emailQueueService = emailQueueService;
            //_remoteAPIService = remoteAPIService;
            //remoteAPISettings = optionsRemoteAPI.Value;
            _converter = converter;
            _userDetailsRepository = userDetailsRepository;
        }

        #region Newsletter Subscriber
        [HttpGet]
        [Route("IsEmailSubscribed")]
        public async Task<ActionResult> IsEmailSubscribed(string email)
        {
            try
            {
                var newsLetterSubscriber = await _newsletterSubscriberService.GetNewsletterSubscriberByEmail(email);
                return Ok(newsLetterSubscriber != null);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("AddToNewsletter")]
        public async Task<IActionResult> Create(NewsletterSubscriberDTO model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.EmailAddress))
                    return Ok(new APIResponseDTO<Int64>
                    {
                        Status = 0,
                        Message = "Email cannot be blank.",
                        Data = 0
                    });

                var newsLetterSubscriber = await _newsletterSubscriberService.GetNewsletterSubscriberByEmail(model.EmailAddress);
                if (newsLetterSubscriber != null)
                    return Ok(new APIResponseDTO<Int64> { 
                        Status = 2,
                        Message = "Email already subscribed to newsletter.",
                        Data = 0
                    });
                Int64 userDetailId = await _userDetailService.GetUserDetailId(GetUserId());
                if(userDetailId > 0)
                {
                    var userdetailData = await _userDetailsRepository.GetSingle(userDetailId);
                    if (userdetailData == null) return Ok(new APIResponseDTO<Int64>
                    {
                        Status = 3,
                        Message = "User Not Found.",
                        Data = 0
                    });
                    model.FirstName = userdetailData.FName;
                    model.LastName = userdetailData.LName;
                    model.CreatedById = userDetailId;
                    model.ModifiedById = userDetailId;
                }
                model.IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                Int64 subscriberId = await _newsletterSubscriberService.Create(model);
                return Ok(new APIResponseDTO<Int64>
                {
                    Status = 1,
                    Message = "Email successfully subscribed to newsletter.",
                    Data = subscriberId
                });
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    
        [HttpPost]
        [Route("GetAllSubscribers")]
        public async Task<IActionResult> GetAllSubscribers(NewsletterSubscriberQueryObject query)
        {
            try
            {
                var result = await _newsletterSubscriberService.NewsletterSubscriberList(query);
                return Ok(result);
            }
            catch(Exception ex)
            {
                throw;
            }
        }


        //[Route("ExportToExcelSubscribers")]
        //[HttpPost]
        //public async Task<FileContentResult> ExportToExcelSubscribers(NewsletterSubscriberQueryObject query)
        //{
        //    try
        //    {

        //        if (query.printall)
        //        {
        //            query.Page = 1;
        //            query.PageSize = int.MaxValue;
        //        }
        //        QueryResult<NewsletterSubscriberDTO> result = await _newsletterSubscriberService.NewsletterSubscriberList(query);

        //        using (var workBook = new XLWorkbook())
        //        {
        //            var workSheet = workBook.Worksheets.Add("Subscribers");
        //            var currentRow = 1;

        //            workSheet.Cell(currentRow, 1).SetValue("ID");
        //            workSheet.Cell(currentRow, 2).SetValue("First Name");
        //            workSheet.Cell(currentRow, 3).SetValue("Last Name");
        //            workSheet.Cell(currentRow, 4).SetValue("Email Address");
        //            workSheet.Cell(currentRow, 5).SetValue("IP Address");
        //            workSheet.Cell(currentRow, 6).SetValue("Status");
        //            workSheet.Cell(currentRow, 7).SetValue("Subscribed On");


        //            foreach (var item in result.Items)
        //            {

        //                currentRow++;
        //                workSheet.Cell(currentRow, 1).SetValue(item.Id);
        //                workSheet.Cell(currentRow, 2).SetValue(item.FirstName);
        //                workSheet.Cell(currentRow, 3).SetValue(item.LastName);
        //                workSheet.Cell(currentRow, 4).SetValue(item.EmailAddress);
        //                workSheet.Cell(currentRow, 5).SetValue(item.IPAddress);
        //                workSheet.Cell(currentRow, 6).SetValue(item.Status == 1 ? "Active" : "InActive");
        //                workSheet.Cell(currentRow, 7).SetValue(item.CreatedOn);
        //            }

        //            using (var stream = new MemoryStream())
        //            {
        //                workBook.SaveAs(stream);
        //                stream.Seek(0, SeekOrigin.Begin);
        //                var content = stream.ToArray();
        //                return File(
        //                    content,
        //                     "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                    "Subscribers.xlsx"
        //                 );

        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }

        //}



        [Route("ExportToPdfSubscribers")]
        [HttpPost]
        public async Task<FileContentResult> ExportToPdfSubscribers(NewsletterSubscriberQueryObject query)
        {
            try
            {

                if (query.printall)
                {
                    query.Page = 1;
                    query.PageSize = int.MaxValue;
                }
                QueryResult<NewsletterSubscriberDTO> result = await _newsletterSubscriberService.NewsletterSubscriberList(query);
                return File(_converter.Convert(PrintPdfHelper.CreateTablePDF(_newsletterSubscriberService.GeneratePdfTemplateString(result))), "application/pdf");
            }
            catch (Exception e)
            {
                throw;
            }

        }

        [HttpPost]
        [Route("DeleteNewsletterSubscribersById/{id}")]
        public async Task<IActionResult> DeleteNewsletterSubscribersById(Int64 id)
        {
            try
            {
                await _newsletterSubscriberService.Delete(id);
                return Ok(true);
            }catch(Exception ex)
            {
                throw;
            }
        }

        [HttpDelete]
        [Route("DeleteNewsletterSubscribersByEmail/{email}")]
        public async Task<IActionResult> DeleteNewsletterSubscribersByEmail(string email)
        {
            try
            {
                var check=await _newsletterSubscriberService.Delete(email);
                return Ok(check);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("ChangeNewsletterSubscriberStatus/{id}")]
        public async Task<IActionResult> ChangeNewsletterSubscriberStatus(Int64 id)
        {
            try
            {
                var result = await _newsletterSubscriberService.UpdateStatus(id);
                return Ok(result);
            }catch(Exception ex)
            {
                throw;
            }
        }
        #endregion Newsletter Subscriber

        #region Newsletter
        [HttpGet]
        [Route("GetNewsletterStatus")]
        public async Task<IActionResult> GetNewsletterStatus()
        {
            try
            {
                List<SelectListItem> items = (from i in Enum.GetValues(typeof(Enums.NewsletterStatus)).Cast<Enums.NewsletterStatus>()
                                              select new SelectListItem() { 
                                                  Value = ((int)i).ToString(),
                                                  Text = i.ToString()

                                              }).ToList();
                return Ok(items);
            }catch(Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetNewsletterById/{id}")]
        public async Task<IActionResult> GetNewsletterById(Int64 id)
        {
            try
            {
                var result = await _newsletterService.GetNewsletterById(id);
                return Ok(result);
            }catch(Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("CreateNewsletter")]
        public async Task<IActionResult> CreateNewsletter(NewsletterDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userDetailId = await _userDetailService.GetUserDetailId(GetUserId());
                    if(model.Id > 0)
                    {
                        model.ModifiedById = userDetailId;
                        if(model.Status == (int)Enums.NewsletterStatus.Confirmed)
                        {
                            model.SendOn = DateTime.Now;
                        }

                        Int64 result = await _newsletterService.Update(model);
                        if(model.Status == (int)Enums.NewsletterStatus.Confirmed || model.Status == (int)Enums.NewsletterStatus.SendNow)
                        {
                            NewsletterAPIDTO apiData = await _newsletterService.GetNewletterForAPI(result);
                            foreach(NewsletterSubscriberAPIDTO subscriber in apiData.Subscribers)
                            {
                                EmailQueueDTO queueModel = new EmailQueueDTO()
                                {
                                    TemplateName = EmailTemplateType.NEWSLETTER.ToString(),
                                    PriorityId = (int)EmailPriority.Medium,
                                    ToEmail = subscriber.Email,
                                    ToName = subscriber.Name,
                                    Subject = model.Subject,
                                    Body = model.EmailContent,
                                    StatusId = (int)EmailStatus.Pending,
                                    Attachements = "",
                                    CreatedOn = DateTime.Now,
                                    QueuedOn = DateTime.Now,
                                    Data = model.Id.ToString()
                                };

                                var id = await _emailQueueService.Create(queueModel);
                            }
                            //await _remoteAPIService.SendNewsletter(apiData, remoteAPISettings);
                        }
                        if(model.Status == (int)Enums.NewsletterStatus.Cancelled)
                        {
                            await _emailQueueService.CancelEmailQueue(model.Id);
                            //await _remoteAPIService.CancelSendNewsletter(model.Id, remoteAPISettings);
                        }
                        return Ok(result);
                    } else
                    {
                        Int64 result = await _newsletterService.Create(model);
                        return Ok(result);
                    }

                }
                return null;
            }catch(Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("UpsertNewsletter")]
        public async Task<IActionResult> UpsertNewsletter(NewsletterDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userDetailId = await _userDetailService.GetUserDetailId(GetUserId());
                    if (model.Id > 0)
                    {
                        model.ModifiedById = userDetailId;
                        if (model.Status == (int)Enums.NewsletterStatus.Confirmed)
                        {
                            model.SendOn = DateTime.Now;
                        }

                        Int64 result = await _newsletterService.Update(model);
                        if (model.Status == (int)Enums.NewsletterStatus.Confirmed || model.Status == (int)Enums.NewsletterStatus.SendNow)
                        {
                            NewsletterAPIDTO apiData = await _newsletterService.GetNewletterForAPI(result);
                            foreach (NewsletterSubscriberAPIDTO subscriber in apiData.Subscribers)
                            {
                                EmailQueueDTO queueModel = new EmailQueueDTO()
                                {
                                    TemplateName = EmailTemplateType.NEWSLETTER.ToString(),
                                    PriorityId = (int)EmailPriority.Medium,
                                    ToEmail = subscriber.Email,
                                    ToName = subscriber.Name,
                                    Subject = model.Subject,
                                    Body = model.EmailContent,
                                    StatusId = (int)EmailStatus.Pending,
                                    Attachements = "",
                                    CreatedOn = DateTime.Now,
                                    QueuedOn = DateTime.Now,
                                    Data = model.Id.ToString()
                                };

                                var id = await _emailQueueService.Create(queueModel);
                            }
                            //await _remoteAPIService.SendNewsletter(apiData, remoteAPISettings);
                        }
                        if (model.Status == (int)Enums.NewsletterStatus.Cancelled)
                        {
                            await _emailQueueService.CancelEmailQueue(model.Id);
                            //await _remoteAPIService.CancelSendNewsletter(model.Id, remoteAPISettings);
                        }
                        return Ok(result);
                    }
                    else
                    {
                        Int64 result = await _newsletterService.Create(model);
                        return Ok(result);
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("GetAllNewsletter")]
        public async Task<IActionResult> GetAllNewsletter(NewsletterQueryObject query)
        {
            try
            {
                var result = await _newsletterService.NewsletterList(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //[Route("ExportToExcel")]
        //[HttpPost]
        //public async Task<FileContentResult> ExportToExcel(NewsletterQueryObject query)
        //{
        //    try
        //    {

        //        if (query.printall)
        //        {
        //            query.Page = 1;
        //            query.PageSize = int.MaxValue;
        //        }
        //        QueryResult<NewsletterDTO> result = await _newsletterService.NewsletterList(query);

        //        using (var workBook = new XLWorkbook())
        //        {
        //            var workSheet = workBook.Worksheets.Add("NewsLetter");
        //            var currentRow = 1;

        //            workSheet.Cell(currentRow, 1).SetValue("ID");
        //            workSheet.Cell(currentRow, 2).SetValue("Name");
        //            workSheet.Cell(currentRow, 3).SetValue("Description");
        //            workSheet.Cell(currentRow, 4).SetValue("Status");
        //            workSheet.Cell(currentRow, 5).SetValue("Send On");



        //            foreach (var item in result.Items)
        //            {
        //                switch (item.Status)
        //                {
        //                    case 10:
        //                        item.statusString = "Draft";
        //                        break;
        //                    case 20:
        //                        item.statusString = "Confirmed";
        //                        break;
        //                    case 30:
        //                        item.statusString = "SendNow";
        //                        break;
        //                    case 40:
        //                        item.statusString = "Cancelled";
        //                        break;
        //                    case 50:
        //                        item.statusString = "Sent";
        //                        break;

        //                }
        //                currentRow++;
        //                workSheet.Cell(currentRow, 1).SetValue(item.Id);
        //                workSheet.Cell(currentRow, 2).SetValue(item.Name);
        //                workSheet.Cell(currentRow, 3).SetValue(item.Description);
        //                workSheet.Cell(currentRow, 4).SetValue(item.statusString);
        //                workSheet.Cell(currentRow, 5).SetValue(item.SendOn);
        //            }

        //            using (var stream = new MemoryStream())
        //            {
        //                workBook.SaveAs(stream);
        //                stream.Seek(0, SeekOrigin.Begin);
        //                var content = stream.ToArray();
        //                return File(
        //                    content,
        //                     "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                    "NewsLetter.xlsx"
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
        public async Task<FileContentResult> ExportToPdf(NewsletterQueryObject query)
        {
            try
            {

                if (query.printall)
                {
                    query.Page = 1;
                    query.PageSize = int.MaxValue;
                }
                QueryResult<NewsletterDTO> result = await _newsletterService.NewsletterList(query);
                return File(_converter.Convert(PrintPdfHelper.CreateTablePDF(_newsletterService.GeneratePdfTemplateString(result))), "application/pdf");
            }
            catch (Exception e)
            {
                throw;
            }

        }

        [HttpPost]
        [Route("DeleteNewsletterById/{id}")]
        public async Task<IActionResult> DeleteNewsletterById(Int64 id)
        {
            try
            {
                await _newsletterService.Delete(id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion Newsletter
    
    }
}
