using Volo.Abp.Modularity;

namespace MySeries;

/* Inherit from this class for your domain layer tests. */
public abstract class MySeriesDomainTestBase<TStartupModule> : MySeriesTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
