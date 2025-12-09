using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace CompanyEmployees.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            //configuration manuel en chargeant appsettings.json
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            //construire les options pour dbcontext avec le provider sql server via la chaine de connexion et génère les migrations dans le projet CompanyEmployees au lieu de Repository (plus clean archi)
            var builder = new DbContextOptionsBuilder<RepositoryContext>().UseSqlServer(configuration.GetConnectionString("sqlConnection"),b=>b.MigrationsAssembly("CompanyEmployees"));
                
            

            return new RepositoryContext(builder.Options);
            
        }
    }
}
