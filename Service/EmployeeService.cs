using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private IRepositoryManager _repository;
        private IMapper _mapper;
        private ILoggerManager _logger;
        public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);        
            var employeesFromCompany = await _repository.EmployeeRepository.GetEmployeesAsync(companyId, trackChanges);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromCompany);
            return employeesDto;
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);        
            var employee = await GetEmployeeAndCheckIfItExists(companyId, employeeId, trackChanges);         
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return employeeDto;

        }

        public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges)
        {
          
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);      
            var employeeEntity = _mapper.Map<Employee>(employeeForCreationDto);
            _repository.EmployeeRepository.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repository.SaveAsync();
            var employeeDto = _mapper.Map<EmployeeDto>(employeeEntity);
            return employeeDto;

        }

        public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);
            var employeeToDelete =  await GetEmployeeAndCheckIfItExists(companyId, id, trackChanges);      
            _repository.EmployeeRepository.DeleteEmployee(employeeToDelete);
            await _repository.SaveAsync();

        }

        public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool comptrackChanges, bool empTrackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, comptrackChanges);          
            var employeeToUpdate = await GetEmployeeAndCheckIfItExists(companyId, id, empTrackChanges);           
            _mapper.Map(employeeForUpdate, employeeToUpdate);
            await _repository.SaveAsync();


        }

        public async Task<(EmployeeForPatchDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool comptrackChanges, bool empTrackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, comptrackChanges);           
            var employeeEntity = await GetEmployeeAndCheckIfItExists(companyId, id, empTrackChanges);          
            var employeeToPatch = _mapper.Map<EmployeeForPatchDto>(employeeEntity);

            return (employeeToPatch: employeeToPatch, employeeEntity: employeeEntity);


        }

        public async Task SaveChangesForPatchAsync(EmployeeForPatchDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repository.SaveAsync();
        }

        private async Task<Employee> GetEmployeeAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
        {
            var employee = await _repository.EmployeeRepository.GetEmployeeAsync(companyId,id, trackChanges);
            if (employee is null)
                throw new EmployeeNotFoundException(id);
            return employee;

        }

       
        private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var company = await _repository.CompanyRepository.GetCompanyByIdAsync(id, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(id);
            return company;
        }
    }
}
