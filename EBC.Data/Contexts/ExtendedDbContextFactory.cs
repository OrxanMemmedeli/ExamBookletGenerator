using EBC.Core.Helpers.StartupFinders;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EBC.Data.Contexts;

public class ExtendedDbContextFactory : IDesignTimeDbContextFactory<ExtendedDbContext>
{
    public ExtendedDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ExtendedDbContext>();

        // Application layihəsinin kök qovluğunu tapmaq
        var basePath = Path.Combine(AppContext.BaseDirectory);

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build(); //IConfigurationRoot obyektini yaradır. 

        string connectionString = ConnectionStringFinder.GetConnectionString(configuration);


        optionsBuilder.UseSqlServer(connectionString, option =>
        {
            option.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null); // connection zamani xeta alinarsa

            option.CommandTimeout(60); // Sorğunun maksimum icra müddətini 60 saniyə olaraq təyin edir
        });


        return new ExtendedDbContext(optionsBuilder.Options);
    }
}