using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace MySeries.Data;

/* This is used if database provider does't define
 * IMySeriesDbSchemaMigrator implementation.
 */
public class NullMySeriesDbSchemaMigrator : IMySeriesDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
