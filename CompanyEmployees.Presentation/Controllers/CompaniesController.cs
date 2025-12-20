using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Entities.Exceptions;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Text.Json;

namespace CompanyEmployees.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    //[ResponseCache(CacheProfileName = "DefaultCacheProfile")]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _manager;
        public CompaniesController(IServiceManager manager)
        {
            _manager = manager;
        }
        [AllowAnonymous]
        [HttpGet]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> GetCompanies([FromQuery] CompanyParameters companyParameters)
        {
            var result = await _manager.CompanyService.GetAllCompaniesAsync(companyParameters, false);
            Response.Headers.Append("X-Pagination", new StringValues(JsonSerializer.Serialize(result.metaData)));
            return Ok(result.companyDtos);

        }
        [AllowAnonymous]
        [HttpGet("{id:guid}", Name = "CompanyById")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Private, MaxAge = 60)]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var result = await _manager.CompanyService.GetCompanyByIdAsync(id, false);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("collection/{ids?}", Name = "CompanyCollection")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Private, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> GetCompanies([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {

            var result = await _manager.CompanyService.GetCompaniesByIdsAsync(ids, false);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [DtoValidationFilter]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {

            var companyDto = await _manager.CompanyService.CreateCompanyAsync(company);
            return CreatedAtRoute("CompanyById", new { id = companyDto.Id }, companyDto);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("collection")]
        [MaxCollectionSize(typeof(CompanyForCreationDto), 10)]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companies)
        {
            if (companies is null)
                throw new CompanyCollectionBadRequest();
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            var result = await _manager.CompanyService.CreateCompanyCollectionAsync(companies);
            return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _manager.CompanyService.DeleteCompanyAsync(id, trackChanges: false);
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        [DtoValidationFilter]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            await _manager.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PatchCompanyForCompany(Guid id, [FromBody] JsonPatchDocument<CompanyForPatchDto> patchDoc)
        {
            if (patchDoc is null)
                throw new DtoBadRequestNullException("PatchDoc object sent by client is null");
            var result = await _manager.CompanyService.GetCompanyForPatchAsync(id, trackChanges: true);
            patchDoc.ApplyTo(result.companyToPatch, ModelState);
            //check also if employeeToPatch is valid for entity
            TryValidateModel(result.companyToPatch);
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _manager.CompanyService.SaveChangesForPatchAsync(result.companyToPatch, result.companyEntity);
            return NoContent();
        }



    }
}
