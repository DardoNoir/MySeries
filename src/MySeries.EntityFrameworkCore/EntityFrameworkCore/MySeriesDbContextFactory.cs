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
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../MySeries.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
