using MySeries.Localization;
using Volo.Abp.Application.Services;

namespace MySeries;

/* Inherit your application services from this class.
 */
public abstract class MySeriesAppService : ApplicationService
{
    protected MySeriesAppService()
    {
        LocalizationResource = typeof(MySeriesResource);
    }
}
