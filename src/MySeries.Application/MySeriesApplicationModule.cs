using Microsoft.Extensions.DependencyInjection;
using MySeries.Application.Contracts;
using MySeries.Notifications;
using MySeries.Qualifications;
using MySeries.Series;
using MySeries.Watchlists;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace MySeries;

[DependsOn(
    typeof(MySeriesDomainModule),
    typeof(MySeriesApplicationContractsModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpAutoMapperModule)
)]
public class MySeriesApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<MySeriesApplicationModule>();
        });

        Configure<AbpBackgroundWorkerOptions>(options =>
        {
            options.IsEnabled = true;
        });

        context.Services.AddTransient<ISeriesApiService, OmdbService>();
    }

    public override async Task OnApplicationInitializationAsync(
        ApplicationInitializationContext context)
    {
        var backgroundWorkerManager =
            context.ServiceProvider.GetRequiredService<IBackgroundWorkerManager>();

        var worker =
            context.ServiceProvider.GetRequiredService<SerieUpdateWorker>();

        await backgroundWorkerManager.AddAsync(worker);
    }
}
