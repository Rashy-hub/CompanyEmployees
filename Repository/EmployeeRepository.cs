using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;

namespace Repository
{
    internal class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            var employeeEntities = await base.FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
                                            .FilterEmployeesByAge(employeeParameters.MinAge, employeeParameters.MaxAge)
                                            .SearchByName(employeeParameters.SearchedTerm)
                                            .OrderByForEmployee(employeeParameters.OrderBy)
                                            .Skip((employeeParameters.pageNumber - 1) * employeeParameters.pageSize)
                                            .Take(employeeParameters.pageSize).ToListAsync();

            var employeeCount = await base.FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).CountAsync();
            return new PagedList<Employee>(employeeEntities, employeeCount, employeeParameters.pageNumber, employeeParameters.pageSize);
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
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
