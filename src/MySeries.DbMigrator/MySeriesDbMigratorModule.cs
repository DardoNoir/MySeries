using MySeries.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MySeries.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(MySeriesEntityFrameworkCoreModule),
    typeof(MySeriesApplicationContractsModule)
)]
public class MySeriesDbMigratorModule : AbpModule
{
}
