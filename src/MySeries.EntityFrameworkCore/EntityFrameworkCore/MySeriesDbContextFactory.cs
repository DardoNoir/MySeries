using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MySeries.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class MySeriesDbContextFactory : IDesignTimeDbContextFactory<MySeriesDbContext>
{
    public MySeriesDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        MySeriesEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<MySeriesDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new MySeriesDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.secrets.json", optional: true)
            .Build();
    }

}
