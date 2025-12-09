using AutoMapper;
using AutoMapper.Internal;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class CompanyService : ICompanyService
    {
        private IRepositoryManager _repository;
        private ILoggerManager _logger;
        private IMapper _mapper;
        public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }


        public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
        {

            var companiesEntities = _repository.CompanyRepository.GetAllCompanies(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companiesEntities);
            return companiesDto;

        }

        public CompanyDto GetCompanyById(Guid id, bool trackChanges)
        {
            var companyEntity = _repository.CompanyRepository.GetCompanyById(id, trackChanges);
            if (companyEntity is null)
                throw new CompanyNotFoundException(id);
            var companyDto = _mapper.Map<CompanyDto>(companyEntity);
            return companyDto;
        }

        public CompanyDto CreateCompany(CompanyForCreationDto company)
        {
            if (company is null)
                throw new CompanyBadRequest();
            var companyEntity = _mapper.Map<Company>(company);
            _repository.CompanyRepository.CreateCompany(companyEntity);
            _repository.Save();
            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return companyToReturn;
        }

        public IEnumerable<CompanyDto> GetCompaniesByIds(IEnumerable<Guid> companyIds, bool trackChanges)
        {
            if (companyIds is null)
                throw new IdParameterBadRequestException();
            var companiesEntities = _repository.CompanyRepository.GetCompaniesByIds(companyIds, trackChanges);
            if(companiesEntities.Count()!= companyIds.Count())
                throw new CollectionByIdsBadRequestException();
            var companiesDtos= _mapper.Map<IEnumerable<CompanyDto>>(companiesEntities);
            return companiesDtos;
        }

        public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();
            var companyEntities=_mapper.Map<IEnumerable<Company>>(companyCollection);

            foreach (Company company in companyEntities)
            {
                _repository.CompanyRepository.CreateCompany(company);
            }

            _repository.Save();

            var companyDtos=_mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var stringIds = string.Join(",", companyDtos.Select((c)=>c.Id));

            return (companies:companyDtos,ids:stringIds);
           
        }

        public void DeleteCompany(Guid id ,bool trackChanges)
        {
          var companyToDelete= _repository.CompanyRepository.GetCompanyById(id, trackChanges);
            if (companyToDelete is null)
                throw new CompanyNotFoundException(id);
          _repository.CompanyRepository.DeleteCompany(companyToDelete);
            _repository.Save();  
        }
    }
}
