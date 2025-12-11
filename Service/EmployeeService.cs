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

        public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
        {
            var company = _repository.CompanyRepository.GetCompanyById(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeesFromCompany = _repository.EmployeeRepository.GetEmployees(companyId, trackChanges);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromCompany);
            return employeesDto;
        }

        public EmployeeDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var company = _repository.CompanyRepository.GetCompanyById(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employee = _repository.EmployeeRepository.GetEmployee(companyId, employeeId, trackChanges);
            if (employee is null)
                throw new EmployeeNotFoundException(employeeId);
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return employeeDto;

        }

        public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges)
        {
            if (employeeForCreationDto is null)
                throw new EmployeeBadRequest();

            var company=_repository.CompanyRepository.GetCompanyById(companyId,trackChanges);
            if(company is null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = _mapper.Map<Employee>(employeeForCreationDto);
            _repository.EmployeeRepository.CreateEmployeeForCompany(companyId, employeeEntity);
            _repository.Save();
            var employeeDto=_mapper.Map<EmployeeDto>(employeeEntity);
            return employeeDto;

        }

        public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.CompanyRepository.GetCompanyById(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeToDelete = _repository.EmployeeRepository.GetEmployee(companyId,id, trackChanges);
            if (employeeToDelete is null)
                throw new EmployeeNotFoundException(id);
            _repository.EmployeeRepository.DeleteEmployee(employeeToDelete);
            _repository.Save();
          
        }

        public void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool comptrackChanges, bool empTrackChanges)
        {
            if (employeeForUpdate is null)
                throw new EmployeeBadRequest();
            var company=_repository.CompanyRepository.GetCompanyById(companyId, comptrackChanges);
            if(company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeToUpdate = _repository.EmployeeRepository.GetEmployee(companyId, id, empTrackChanges);
            if(employeeToUpdate is null)
                throw new EmployeeNotFoundException(id);          
            _mapper.Map(employeeForUpdate,employeeToUpdate);
            _repository.Save();

           
        }

        public (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) GetEmployeeForPatch(Guid companyId, Guid id, bool comptrackChanges, bool empTrackChanges)
        {
            var company=_repository.CompanyRepository.GetCompanyById(companyId,comptrackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeEntity = _repository.EmployeeRepository.GetEmployee(companyId, id, empTrackChanges);
            if (employeeEntity is null)
                throw new EmployeeNotFoundException(id);
            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

            return (employeeToPatch: employeeToPatch, employeeEntity:employeeEntity);
            
               
        }

        public void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
           _mapper.Map(employeeToPatch, employeeEntity);
           _repository.Save(); 
        }
    }
}
