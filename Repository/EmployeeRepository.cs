using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    internal class EmployeeRepository : RepositoryBase<Employee>,IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext context) : base(context)
        {
        }

        public IEnumerable<Employee> GetEmployees(Guid companyId,bool trackChanges)
        {
            return base.FindByCondition(c=>c.CompanyId.Equals(companyId),trackChanges).ToList();
        }

        public Employee GetEmployee(Guid companyId,Guid employeeId,bool trackChanges)
        {
            return base.FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackChanges).SingleOrDefault();
        }

        public void CreateEmployeeForCompany(Guid companyId,Employee employee)
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
