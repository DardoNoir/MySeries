using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySeries.Data;
using Volo.Abp.DependencyInjection;

namespace MySeries.EntityFrameworkCore;

public class EntityFrameworkCoreMySeriesDbSchemaMigrator
    : IMySeriesDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreMySeriesDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the MySeriesDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<MySeriesDbContext>()
            .Database
            .MigrateAsync();
    }
}
