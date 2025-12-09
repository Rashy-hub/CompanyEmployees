using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.MapperProfiles
{
    public class MapperConfig:Profile    
    {
        public MapperConfig()
        {
            CreateMap<Company, CompanyDto>().ForMember(c => c.FullAddress,opt => opt.MapFrom(x => string.Join(' ', x.Adress, x.Country)));
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<EmployeeForUpdateDto, Employee>();

        }
    }
}
