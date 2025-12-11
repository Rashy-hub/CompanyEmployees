using CompanyEmployees.Presentation.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _manager;
        public CompaniesController(IServiceManager manager)
        {
            _manager = manager;
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {

            var result = await _manager.CompanyService.GetAllCompaniesAsync(false);
            return Ok(result);


        }
        [HttpGet("{id:guid}", Name = "CompanyById")]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var result = await _manager.CompanyService.GetCompanyByIdAsync(id, false);
            return Ok(result);
        }

        [HttpGet("collection/{ids?}", Name = "CompanyCollection")]

        public async Task<IActionResult> GetCompanies([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var result = await _manager.CompanyService.GetCompaniesByIdsAsync(ids, false);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            var companyDto = await _manager.CompanyService.CreateCompanyAsync(company);
            return CreatedAtRoute("CompanyById", new { id = companyDto.Id }, companyDto);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companies)
        {

            var result = await _manager.CompanyService.CreateCompanyCollectionAsync(companies);
            return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _manager.CompanyService.DeleteCompanyAsync(id, trackChanges: false);
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            await _manager.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);
            return NoContent();
        }

    }
}
