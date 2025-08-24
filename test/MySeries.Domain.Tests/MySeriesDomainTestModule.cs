using Volo.Abp.Modularity;

namespace MySeries;

[DependsOn(
    typeof(MySeriesDomainModule),
    typeof(MySeriesTestBaseModule)
)]
public class MySeriesDomainTestModule : AbpModule
{

}
