using Entities.Models;


namespace Repository.Extensions
{
    internal static class EmployeeRepositoryExtensions
    {
        public static IQueryable<Employee> FilterEmployeesByAge(this IQueryable<Employee> employees, uint MinAge, uint MaxAge)
        {
            return employees.Where(e => (e.Age > MinAge && e.Age < MaxAge));
        }
        public static IQueryable<Employee> SearchByName(this IQueryable<Employee> employees, string SearchTerm)
        {
            if (string.IsNullOrEmpty(SearchTerm))
                return employees;
            var lowerSearchTerm = SearchTerm.Trim().ToLower();
            return employees.Where(e => e.Name.ToLower().Contains(SearchTerm));
        }
    }
}
