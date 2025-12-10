namespace Shared.DataTransferObjects
{
    public record CompanyForUpdateDto
    {
        public string? Name { get; init; }
        public string? Adress { get; init; }
        public string? Country { get; init; }

        // if provided we can directly update children , EF core (and automapper) will understand it and take care of it 
        public IEnumerable<EmployeeForCreationDto>? Employees { get; init; }
    }
}
