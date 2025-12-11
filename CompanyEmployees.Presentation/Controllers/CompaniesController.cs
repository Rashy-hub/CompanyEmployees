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
        public IActionResult GetCompanies()
        {

            var result = _manager.CompanyService.GetAllCompanies(false);
            return Ok(result);


        }
        [HttpGet("{id:guid}", Name = "CompanyById")]
        public IActionResult GetCompany(Guid id)
        {
            var result = _manager.CompanyService.GetCompanyById(id, false);
            return Ok(result);
        }

        [HttpGet("collection/{ids?}", Name = "CompanyCollection")]

        public IActionResult GetCompanies([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var result = _manager.CompanyService.GetCompaniesByIds(ids, false);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            var companyDto = _manager.CompanyService.CreateCompany(company);
            return CreatedAtRoute("CompanyById", new { id = companyDto.Id }, companyDto);
        }

        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companies)
        {

            var result = _manager.CompanyService.CreateCompanyCollection(companies);
            return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteCompany(Guid id) 
        {
            _manager.CompanyService.DeleteCompany(id, trackChanges: false);
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        public IActionResult UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company )
        {
            _manager.CompanyService.UpdateCompany(id,company,trackChanges: true);
            return NoContent();
        }

    }
}
