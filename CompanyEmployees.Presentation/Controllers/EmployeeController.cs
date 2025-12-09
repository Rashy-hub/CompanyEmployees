using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _manager;
        public EmployeesController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetEmployees(Guid companyId)
        {
            var result = _manager.EmployeeService.GetEmployees(companyId, false);
            return Ok(result);
        }
        [HttpGet("{id:guid}", Name = "EmployeeById")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {

            var result = _manager.EmployeeService.GetEmployee(companyId, id, false);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreationDto)
        {

            var result = _manager.EmployeeService.CreateEmployeeForCompany(companyId, employeeForCreationDto, trackChanges: false);
            return CreatedAtRoute("EmployeeById", new { companyId, id = result.Id }, result);

        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteEmployeeForCompany(Guid companyId,Guid id) 
        {
            _manager.EmployeeService.DeleteEmployeeForCompany(companyId,id, trackChanges:false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateEmployeeForCompany(Guid companyId,Guid id, [FromBody] EmployeeForUpdateDto employeeForUpdateDto)
        {
            _manager.EmployeeService.UpdateEmployeeForCompany(companyId, id, employeeForUpdateDto, comptrackChanges: false, empTrackChanges: true);
            return NoContent();
        }

    }
}
