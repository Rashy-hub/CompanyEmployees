using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies(bool trackChanges);
        IEnumerable<Company> GetCompaniesByIds(IEnumerable<Guid> Ids, bool trackChanges);
        Company GetCompanyById(Guid id , bool trackChanges);
        void CreateCompany(Company company);

        void DeleteCompany(Company company);

       


       
    }
}
