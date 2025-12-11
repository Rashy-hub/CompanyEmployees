using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    internal class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Employee>> GetEmployees(Guid companyId, bool trackChanges)
        {
            return await base.FindByCondition(c => c.CompanyId.Equals(companyId), trackChanges).ToListAsync();
        }

        public async Task<Employee> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
        {
            return await base.FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackChanges).SingleOrDefaultAsync();
        }

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            base.Create(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            base.Delete(employee);
        }
    }
}
