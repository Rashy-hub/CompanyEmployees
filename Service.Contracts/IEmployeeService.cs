using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);
        public EmployeeDto GetEmployee(Guid employeeId, Guid companyId,bool trackChanges);
        public EmployeeDto CreateEmployeeForCompany(Guid companyId,EmployeeForCreationDto employeeForCreationDto , bool trackChanges);
        public void DeleteEmployeeForCompany (Guid companyId, Guid id , bool trackChanges);
    }
}
