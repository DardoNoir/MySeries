using System.Threading.Tasks;

namespace MySeries.Data;

public interface IMySeriesDbSchemaMigrator
{
    Task MigrateAsync();
}
