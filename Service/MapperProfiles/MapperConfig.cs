using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.MapperProfiles
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            // =========================
            // READ (Entity -> DTO)
            // =========================
            CreateMap<Company, CompanyDto>()
                .ForMember(
                    c => c.FullAddress,
                    opt => opt.MapFrom(x => string.Join(' ', x.Adress, x.Country))
                );

            CreateMap<Employee, EmployeeDto>();

            // =========================
            // CREATE (POST)
            // =========================
            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<EmployeeForCreationDto, Employee>();

            // =========================
            // UPDATE (PUT - full replace)
            // =========================
            CreateMap<CompanyForUpdateDto, Company>();
            CreateMap<EmployeeForUpdateDto, Employee>();

            // =========================
            // PATCH (partial update)
            // =========================

            // Company PATCH: ignore null values (true PATCH behavior)
            CreateMap<CompanyForPatchDto, Company>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null)
                );

            // Employee PATCH: ignore null values (true PATCH behavior)
            CreateMap<EmployeeForPatchDto, Employee>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null)
                );

            // PATCH reverse mappings (Entity -> PatchDto)
            CreateMap<Company, CompanyForPatchDto>();
            CreateMap<Employee, EmployeeForPatchDto>();

            // For User Registration
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
