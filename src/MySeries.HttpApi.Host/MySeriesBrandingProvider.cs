using Microsoft.Extensions.Localization;
using MySeries.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace MySeries;

[Dependency(ReplaceServices = true)]
public class MySeriesBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<MySeriesResource> _localizer;

    public MySeriesBrandingProvider(IStringLocalizer<MySeriesResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
