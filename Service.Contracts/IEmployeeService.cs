using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges);
        Task<EmployeeDto> GetEmployeeAsync(Guid employeeId, Guid companyId,bool trackChanges);
        Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId,EmployeeForCreationDto employeeForCreationDto , bool trackChanges);
        Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id , bool trackChanges);
        Task UpdateEmployeeForCompanyAsync(Guid companyId,Guid id,EmployeeForUpdateDto employeeForUpdate, bool comptrackChanges,bool empTrackChanges);
        Task<(EmployeeForPatchDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool comptrackChanges, bool empTrackChanges);
        Task SaveChangesForPatchAsync(EmployeeForPatchDto employeeToPatch, Employee employeeEntity);
    }
}
