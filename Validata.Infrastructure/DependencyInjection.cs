using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Validata.Infrastructure.Infrastructure;
using Validata.Infrastructure.Repositories;

namespace Validata.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IAppDbContext, AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void MigrateTables(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

            var dbFacade = context.GetDatabaseFacade();
            dbFacade.OpenConnection();

            var connection = dbFacade.GetDbConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT count(*) AS TOTALNUMBEROFTABLES
                                         FROM INFORMATION_SCHEMA.TABLES
                                         WHERE TABLE_SCHEMA = 'ValidataDB'";
            command.CommandType = CommandType.Text;

            var tableCount = (Int32?)command.ExecuteScalar();

            var appliedMigrations = dbFacade.GetAppliedMigrations();
            var pendingMigrations = dbFacade.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                var a = dbFacade.GetMigrations();
                dbFacade.Migrate();
            }
        }
    }
}
