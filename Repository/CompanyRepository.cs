using Contracts;
using Entities.Models;

namespace Repository
{
    internal class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {

        public CompanyRepository(RepositoryContext context) : base(context)
        {

        }            

        public IEnumerable<Company> GetAllCompanies(bool trackChanges)
        {
            return base.FindAll(trackChanges).OrderBy(c => c.Name).ToList();
        }
        public Company GetCompanyById(Guid companyId, bool trackChanges)
        {

            return base.FindByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefault();
        }
        public void CreateCompany(Company company)
        {
           base.Create(company);
        }

        public IEnumerable<Company> GetCompaniesByIds(IEnumerable<Guid> Ids, bool trackChanges)
        {
           
           return base.FindByCondition(c=>Ids.Contains(c.Id),trackChanges).ToList();
        }

        public void DeleteCompany(Company company)
        {
            base.Delete(company);
        }
    }
}
