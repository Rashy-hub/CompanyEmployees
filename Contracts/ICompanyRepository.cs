using Entities.Models;

namespace Contracts
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllCompanies(bool trackChanges);
        Task<IEnumerable<Company>> GetCompaniesByIds(IEnumerable<Guid> Ids, bool trackChanges);
        Task<Company> GetCompanyById(Guid id, bool trackChanges);
        void CreateCompany(Company company);

        void DeleteCompany(Company company);

    }
}
