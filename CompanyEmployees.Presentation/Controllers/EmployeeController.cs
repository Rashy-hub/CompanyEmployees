using CompanyEmployees.Presentation.ActionFilters;
using Entities.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Text.Json;

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
        public async Task<IActionResult> GetEmployees(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            var result = await _manager.EmployeeService.GetEmployeesAsync(companyId,employeeParameters, false);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(result.metaData));
            return Ok(result.employeeDtos);
        }
        [HttpGet("{id:guid}", Name = "EmployeeById")]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
        {

            var result = await _manager.EmployeeService.GetEmployeeAsync(companyId, id, false);
            return Ok(result);
        }
        [HttpPost]
        [DtoValidationFilter]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreationDto)
        {
            var result = await _manager.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employeeForCreationDto, trackChanges: false);
            return CreatedAtRoute("EmployeeById", new { companyId, id = result.Id }, result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            await _manager.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, id, trackChanges: false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [DtoValidationFilter]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employeeForUpdateDto)
        {
            await _manager.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id, employeeForUpdateDto, comptrackChanges: false, empTrackChanges: true);
            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PatchEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForPatchDto> patchDoc)
        {
            if (patchDoc is null)
                throw new DtoBadRequestNullException("PatchDoc object sent by client is null");
            var result = await _manager.EmployeeService.GetEmployeeForPatchAsync(companyId, id, comptrackChanges: false, empTrackChanges: true);
            patchDoc.ApplyTo(result.employeeToPatch, ModelState);
            //check also if employeeToPatch is valid for entity
            TryValidateModel(result.employeeToPatch);
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _manager.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch, result.employeeEntity);
            return NoContent();
        }

    }
}
