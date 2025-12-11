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
            var company = await _repository.CompanyRepository.GetCompanyByIdAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeesFromCompany = await _repository.EmployeeRepository.GetEmployeesAsync(companyId, trackChanges);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromCompany);
            return employeesDto;
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var company = await _repository.CompanyRepository.GetCompanyByIdAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employee = await _repository.EmployeeRepository.GetEmployeeAsync(companyId, employeeId, trackChanges);
            if (employee is null)
                throw new EmployeeNotFoundException(employeeId);
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return employeeDto;

        }

        public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges)
        {
            if (employeeForCreationDto is null)
                throw new EmployeeBadRequest();

            var company = await _repository.CompanyRepository.GetCompanyByIdAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = _mapper.Map<Employee>(employeeForCreationDto);
            _repository.EmployeeRepository.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repository.SaveAsync();
            var employeeDto = _mapper.Map<EmployeeDto>(employeeEntity);
            return employeeDto;

        }

        public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
        {
            var company = await _repository.CompanyRepository.GetCompanyByIdAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeToDelete = await _repository.EmployeeRepository.GetEmployeeAsync(companyId, id, trackChanges);
            if (employeeToDelete is null)
                throw new EmployeeNotFoundException(id);
            _repository.EmployeeRepository.DeleteEmployee(employeeToDelete);
            await _repository.SaveAsync();

        }

        public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool comptrackChanges, bool empTrackChanges)
        {
            if (employeeForUpdate is null)
                throw new EmployeeBadRequest();
            var company = await _repository.CompanyRepository.GetCompanyByIdAsync(companyId, comptrackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeToUpdate = await _repository.EmployeeRepository.GetEmployeeAsync(companyId, id, empTrackChanges);
            if (employeeToUpdate is null)
                throw new EmployeeNotFoundException(id);
            _mapper.Map(employeeForUpdate, employeeToUpdate);
            await _repository.SaveAsync();


        }

        public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool comptrackChanges, bool empTrackChanges)
        {
            var company = await _repository.CompanyRepository.GetCompanyByIdAsync(companyId, comptrackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeEntity = await _repository.EmployeeRepository.GetEmployeeAsync(companyId, id, empTrackChanges);
            if (employeeEntity is null)
                throw new EmployeeNotFoundException(id);
            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

            return (employeeToPatch: employeeToPatch, employeeEntity: employeeEntity);


        }

        public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repository.SaveAsync();
        }
    }
}
