using AutoMapper;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Repository.IRepository;
using BhoomiGlobalAPI.Service.IService;

namespace BhoomiGlobalAPI.Service
{
    public class ContactService: IContactService
    {
        public IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ContactService(IContactRepository contactRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Add(ContactDTO model)
        {
            try
            {
                var data = _mapper.Map<Contact>(model);
                data.InquiryDate = DateTime.Now;
                await _contactRepository.Add(data);
                await _unitOfWork.Commit();
                return data.Id; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> Delete(long id)
        {
            try
            {
                var data = _contactRepository.GetById(id);
                if(data == null) return false;

                _contactRepository.Delete(data);
                await _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ContactDTO> GetAll()
        {
            try
            {
                var data = _contactRepository.GetAll().OrderByDescending(x => x.Id);
                return _mapper.Map<List<ContactDTO>>(data);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ContactDTO GetById(long id)
        {
            try
            {
                var data = _contactRepository.GetById(id);
                return _mapper.Map<ContactDTO>(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
