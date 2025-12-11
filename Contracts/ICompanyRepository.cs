using Entities.Models;

namespace Contracts
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges);
        Task<IEnumerable<Company>> GetCompaniesByIdsAsync(IEnumerable<Guid> Ids, bool trackChanges);
        Task<Company> GetCompanyByIdAsync(Guid id, bool trackChanges);
        void CreateCompany(Company company);

        void DeleteCompany(Company company);

    }
}
