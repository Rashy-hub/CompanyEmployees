using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts
{
    public interface ICompanyService
    {
        Task<(IEnumerable<CompanyDto> companyDtos,MetaData metaData)> GetAllCompaniesAsync(CompanyParameters companyParameters,bool trackChanges);
        Task<CompanyDto> GetCompanyByIdAsync(Guid id, bool trackChanges);

        Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company);

        Task<IEnumerable<CompanyDto>> GetCompaniesByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges);

        Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection);

        Task DeleteCompanyAsync(Guid id, bool trackChanges);

        Task UpdateCompanyAsync(Guid id, CompanyForUpdateDto company, bool trackChanges);

        Task<(CompanyForPatchDto companyToPatch, Company companyEntity)> GetCompanyForPatchAsync(Guid id, bool trackChanges);
        Task SaveChangesForPatchAsync(CompanyForPatchDto companyToPatch, Company companyEntity);
    }
}
