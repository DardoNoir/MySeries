using Volo.Abp.Modularity;

namespace MySeries;

[DependsOn(
    typeof(MySeriesApplicationModule),
    typeof(MySeriesDomainTestModule)
)]
public class MySeriesApplicationTestModule : AbpModule
{

}
