using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repository
{
    internal class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {

        public CompanyRepository(RepositoryContext context) : base(context)
        {

        }

        public async Task<PagedList<Company>> GetAllCompaniesAsync(CompanyParameters companyParameters,bool trackChanges)
        {
            var companyEntities= await base.FindAll(trackChanges).OrderBy(c => c.Name).Skip((companyParameters.pageNumber-1)*companyParameters.pageSize).Take(companyParameters.pageSize).ToListAsync();
            var companyCount = await base.FindAll(trackChanges).CountAsync();

            return new PagedList<Company> (companyEntities, companyCount,companyParameters.pageNumber,companyParameters.pageSize);
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
