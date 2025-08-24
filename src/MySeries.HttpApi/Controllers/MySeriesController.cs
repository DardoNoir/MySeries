using MySeries.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MySeries.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class MySeriesController : AbpControllerBase
{
    protected MySeriesController()
    {
        LocalizationResource = typeof(MySeriesResource);
    }
}
