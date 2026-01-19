using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;
using Microsoft.Extensions.DependencyInjection;
using MySeries.Series;
using MySeries.Watchlists;
using MySeries.Qualifications;
using MySeries.Notifications;
using MySeries.WatchLists;

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

        context.Services.AddTransient<ISeriesApiService, OmdbService>();
        context.Services.AddTransient<IWatchlistsAppService, WatchlistsAppService>();
        context.Services.AddTransient<IQualificationsAppService, QualificationsAppService>();
        context.Services.AddTransient<INotificationsAppService, NotificationsAppService>();
    }
}