using Volo.Abp.Modularity;

namespace MySeries;

public abstract class MySeriesApplicationTestBase<TStartupModule> : MySeriesTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
