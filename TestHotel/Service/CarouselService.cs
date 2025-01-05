using AutoMapper;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.HelperClass;
using BhoomiGlobalAPI.Repository.IRepository;
using BhoomiGlobalAPI.Service.IService;

namespace BhoomiGlobalAPI.Service
{
    public class CarouselService:ICarouselService
    {
        ICarouselRepository _carouselRepository;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        public CarouselService(
            ICarouselRepository carouselRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _carouselRepository = carouselRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<QueryResult<CarouselModifiedDTO>> GetQueryCarousels(CarouselQueryObject query)
        {
            var carousels = _carouselRepository.GetAll();

            if (!string.IsNullOrEmpty(query.SearchString))
            {
                carousels = carousels.Where(x => x.Name.ToLower().Contains(query.SearchString.ToLower()));
            }
            switch (query.Type)
            {
                case (int)Enums.CarouselType.Brandhome:
                    carousels = carousels.Where(x => x.CarouselType == (int)Enums.CarouselType.Brandhome);
                    break;
                case (int)Enums.CarouselType.Homepage:
                    carousels = carousels.Where(x => x.CarouselType == (int)Enums.CarouselType.Homepage);
                    break;
                case (int)Enums.CarouselType.Storehome:
                    carousels = carousels.Where(x => x.CarouselType == (int)Enums.CarouselType.Storehome);
                    break;

            }

            var pagedObject = carousels
                                .Skip((query.Page - 1) * query.PageSize)
                                .Take(query.PageSize)
                                .OrderBy(c => c.OrderBy)
                                .ThenBy(c => c.Name);
            var resultant = pagedObject.Select(_carousel => new CarouselModifiedDTO
            {
                Id = _carousel.Id,
                Name = _carousel.Name,
                Description = _carousel.Description,
                CarouselType = _carousel.CarouselType ?? 0,
                CarouselType_ = ((Enums.CarouselType)_carousel.CarouselType).ToString(),
                TargetUrl = _carousel.TargetUrl,
                OrderBy = _carousel.OrderBy,
                ImageUrl = "Uploads" + "\\" + _carousel.ImagePath
            });

            var queryResult = new QueryResult<CarouselModifiedDTO>
            {
                TotalItems = carousels.Count(),
                Items = resultant
            };
            return queryResult;
        }


        public async Task<bool> PatchOrders(List<PatchOrderDTO> patchorders)
        {
            try
            {
                foreach (var item in patchorders)
                {
                    var carousel = await _carouselRepository.GetSingle(item.Id);
                    if (carousel != null)
                    {
                        carousel.OrderBy = item.Order;
                    }
                }
                await _unitOfWork.Commit();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }



        public async Task<int> Create(CarouselFewerItemsDTO model)
        {
            try
            {
                var carousel = new Carousel()
                {
                    Name = model.Name,
                    Description = model.Description,
                    OrderBy = CreateOrderHelper(model),
                    CarouselType = model.CarouselType,
                    TargetUrl = model.TargetUrl,
                    Slider = model.Slider
                };

                await _carouselRepository.Add(carousel);
                await _unitOfWork.Commit();
                return carousel.Id;

            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public async Task<int> Update(CarouselFewerItemsDTO model)
        {

            var carousel = await _carouselRepository.GetSingle(model.Id);
            if (carousel != null)
            {
                carousel.Id = model.Id;
                carousel.Name = model.Name;
                carousel.Description = model.Description;
                carousel.CarouselType = model.CarouselType;
                carousel.TargetUrl = model.TargetUrl;
                carousel.Slider = model.Slider;
                await _unitOfWork.Commit();
            }
            return carousel.Id;
        }




        public async Task Delete(int id)
        {
            var carousel = await _carouselRepository.GetSingle(id);
            _carouselRepository.Delete(carousel);

            await _unitOfWork.Commit();
        }




        public async Task<CarouselModifiedDTO> GetCarouselById(int id)
        {
            try
            {
                var _carousel = await _carouselRepository.GetSingle(id);
                var carousel = new CarouselModifiedDTO()
                {
                    Id = _carousel.Id,
                    Name = _carousel.Name,
                    Description = _carousel.Description,
                    CarouselType = _carousel.CarouselType ?? 0,
                    TargetUrl = _carousel.TargetUrl,
                    ImageUrl = "Uploads" + "\\" + _carousel.ImagePath,
                    Slider = _carousel.Slider,
                    HomeBannerSmall = "Uploads" + "\\" + _carousel.homeBannerSmall,
                };
                return carousel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task UploadImage(int carouselId, string filepath)
        {
            var carousel = await _carouselRepository.GetSingle(carouselId);
            carousel.ImagePath = filepath;
            await _unitOfWork.Commit();
        }



        public async Task UploadHomeBannerSmall(int carouselId, string filepath)
        {
            var carousel = await _carouselRepository.GetSingle(carouselId);
            carousel.homeBannerSmall = filepath;
            await _unitOfWork.Commit();
        }




        public CarouselImageUrlDTO GetCarouselImage(int carouselId)
        {
            var car = _carouselRepository.GetById(x => x.Id == carouselId);
            if (car != null)
            {
                var _car = new CarouselImageUrlDTO()
                {
                    Id = carouselId,
                    ImageUrl = car.ImagePath == null ? null : "Uploads" + "\\" + car.ImagePath
                };
                return _car;
            }
            return new CarouselImageUrlDTO();
        }


        public CarouselImageUrlDTO GetCarouselHomeBannerSmallImage(int carouselId)
        {
            var car = _carouselRepository.GetById(x => x.Id == carouselId);
            if (car != null)
            {
                var _car = new CarouselImageUrlDTO()
                {
                    Id = carouselId,
                    ImageUrl = car.homeBannerSmall == null ? null : "Uploads" + "\\" + car.homeBannerSmall
                };
                return _car;
            }
            return new CarouselImageUrlDTO();
        }



        public CarouselImagePathDTO GetCarouselImagePath(int carouselId)
        {
            var car = _carouselRepository.GetById(x => x.Id == carouselId);
            var _car = new CarouselImagePathDTO()
            {
                Id = carouselId,
                ImagePath = car.ImagePath
            };
            return _car;
        }


        public CarouselImagePathDTO GetCarouselHomeBannerSmallImagePath(int carouselId)
        {
            var car = _carouselRepository.GetById(x => x.Id == carouselId);
            var _car = new CarouselImagePathDTO()
            {
                Id = carouselId,
                ImagePath = car.homeBannerSmall
            };
            return _car;
        }

        public string GetImageName(int Id)
        {
            return _carouselRepository.GetById(x => x.Id == Id).ImagePath;
        }


        public string GetHomeBannerSmallImageName(int Id)
        {
            return _carouselRepository.GetById(x => x.Id == Id).homeBannerSmall;
        }


        public List<CarouselStringifiedDTO> GetAll(int storeId, long brandId, int affiliateId, int homepageId)
        {
            var carousels = _carouselRepository.GetAll();
          
            carousels = carousels.Where(x => x.CarouselType == (int)Enums.CarouselType.Homepage);
            var finalrecord = carousels
                            .Select(x => new CarouselStringifiedDTO
                            {
                                Id = x.Id,
                                Name = x.Name,
                                CarouselType = ((Enums.CarouselType)x.CarouselType).ToString(),
                                Description = x.Description,
                                ImageUrl = "Uploads" + "\\" + x.ImagePath,
                                HomeBannerSmall = "Uploads" + "\\" + x.homeBannerSmall,
                                Slider = x.Slider,
                                TargetUrl = x.TargetUrl,
                                OrderBy = x.OrderBy
                            }).OrderBy(x => x.OrderBy);

            return _mapper.Map<List<CarouselStringifiedDTO>>(finalrecord);
        }



        public List<CarouselStringifiedDTO> GetAllByType(int typeId)
        {
            var carousels = _carouselRepository.GetAll().Where(x => x.CarouselType == typeId)
                            .Select(x => new CarouselStringifiedDTO
                            {
                                Id = x.Id,
                                Name = x.Name,
                                CarouselType = ((Enums.CarouselType)x.CarouselType).ToString(),
                                Description = x.Description,
                                ImageUrl = "Uploads" + "/" + x.ImagePath,
                                HomeBannerSmall = "Uploads" + "/" + x.homeBannerSmall,
                                Slider = x.Slider,
                                TargetUrl = x.TargetUrl,
                                OrderBy = x.OrderBy,
                                TargetModule = x.TargetModule,
                                CategoryId = x.CategoryId,
                                EntityId = x.EntityId
                            });

            return _mapper.Map<List<CarouselStringifiedDTO>>(carousels);
        }


        public async Task PatchCarousel(CarouselMobileDTO carmob)
        {
            try
            {
                var carousel = await _carouselRepository.GetSingle(carmob.Id);
                carousel.TargetModule = carmob.TargetModule;
                carousel.EntityId = carmob.EntityId;
                carousel.CategoryId = carmob.CategoryId;
                await _unitOfWork.Commit();
            }
            catch (TaskCanceledException excep)
            {
                throw excep;
            }
        }

        public async Task ArrangeOrder(List<SliderOrder> SliderOrder)
        {
            var result = _carouselRepository.FindBy(x => x.CarouselType == (int)Enums.CarouselType.Homepage);
            var firstOrDefault = SliderOrder.FirstOrDefault();
            foreach (var item in SliderOrder)
            {
                var result1 = result.ToList();
                var checkorder = result.FirstOrDefault(x => x.Id == item.Id);
                if (checkorder != null)
                {
                    checkorder.OrderBy = item.Order;
                }
            }
            await _unitOfWork.Commit();
        }


        public void SaveChanges()
        {
            this._unitOfWork.Commit();
        }


        private int CreateOrderHelper(CarouselFewerItemsDTO model)
        {
            var slider = _carouselRepository.GetAll();
            slider = slider.Where(x => x.CarouselType == (int)Enums.CarouselType.Homepage);
            
            var maximumOrder = 0;
            if (slider.Any())
            {
                maximumOrder = slider.Max(x => x.OrderBy);
            }
            return maximumOrder + 1;
        }
    }
}
