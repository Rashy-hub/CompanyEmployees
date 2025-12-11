using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    internal class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {

        public CompanyRepository(RepositoryContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges)
        {
            return await base.FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();
        }
        public async Task<Company> GetCompanyByIdAsync(Guid companyId, bool trackChanges)
        {

            return await base.FindByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Company>> GetCompaniesByIdsAsync(IEnumerable<Guid> Ids, bool trackChanges)
        {

            return await base.FindByCondition(c => Ids.Contains(c.Id), trackChanges).ToListAsync();
        }

        public void CreateCompany(Company company)
        {
            base.Create(company);
        }


        public void DeleteCompany(Company company)
        {
            base.Delete(company);
        }
    }
}
