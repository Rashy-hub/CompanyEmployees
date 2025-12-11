using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);
        EmployeeDto GetEmployee(Guid employeeId, Guid companyId,bool trackChanges);
        EmployeeDto CreateEmployeeForCompany(Guid companyId,EmployeeForCreationDto employeeForCreationDto , bool trackChanges);
        void DeleteEmployeeForCompany (Guid companyId, Guid id , bool trackChanges);
        void UpdateEmployeeForCompany(Guid companyId,Guid id,EmployeeForUpdateDto employeeForUpdate, bool comptrackChanges,bool empTrackChanges);
        (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) GetEmployeeForPatch(Guid companyId, Guid id, bool comptrackChanges, bool empTrackChanges);
        void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);
    }
}
