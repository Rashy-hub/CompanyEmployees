namespace Shared.RequestFeatures
{
    public class EmployeeParameters : RequestParameters
    {
        public uint MinAge { get; set; }
        public uint MaxAge { get; set; } = uint.MaxValue;
        public bool ValidAgeRange => MinAge < MaxAge;
        public string? SearchedTerm { get; set; }
        public EmployeeParameters() 
        {
            OrderBy = "name";
        }
    }
}
