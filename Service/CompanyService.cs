using AutoMapper;
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


        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
        {

            var companiesEntities = await _repository.CompanyRepository.GetAllCompaniesAsync(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companiesEntities);
            return companiesDto;

        }

        public async Task<CompanyDto> GetCompanyByIdAsync(Guid id, bool trackChanges)
        {
            var companyEntity = await  GetCompanyAndCheckIfItExists(id, trackChanges);
            var companyDto = _mapper.Map<CompanyDto>(companyEntity);
            return companyDto;
        }

        public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);
            _repository.CompanyRepository.CreateCompany(companyEntity);
            await _repository.SaveAsync();
            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return companyToReturn;
        }

        public async Task<IEnumerable<CompanyDto>> GetCompaniesByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges)
        {
            if (companyIds is null)
                throw new IdParameterBadRequestException();
            var companiesEntities = await _repository.CompanyRepository.GetCompaniesByIdsAsync(companyIds, trackChanges);
            if (companiesEntities.Count() != companyIds.Count())
                throw new CollectionByIdsBadRequestException();
            var companiesDtos = _mapper.Map<IEnumerable<CompanyDto>>(companiesEntities);
            return companiesDtos;
        }

        public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);

            foreach (Company company in companyEntities)
            {
                _repository.CompanyRepository.CreateCompany(company);
            }

            await _repository.SaveAsync();

            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var stringIds = string.Join(",", companyDtos.Select((c) => c.Id));

            return (companies: companyDtos, ids: stringIds);

        }

        public async Task DeleteCompanyAsync(Guid id, bool trackChanges)
        {
            var companyToDelete = await GetCompanyAndCheckIfItExists(id, trackChanges);
            if (companyToDelete is null)
                throw new CompanyNotFoundException(id);
            _repository.CompanyRepository.DeleteCompany(companyToDelete);
            await _repository.SaveAsync();
        }

        public async Task UpdateCompanyAsync(Guid id, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            var companyToUpdate = await GetCompanyAndCheckIfItExists(id, trackChanges);
            _mapper.Map(companyForUpdate, companyToUpdate);
            await _repository.SaveAsync();
        }

        private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var company = await _repository.CompanyRepository.GetCompanyByIdAsync(id, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(id);
            return company;
        }
    }
}
