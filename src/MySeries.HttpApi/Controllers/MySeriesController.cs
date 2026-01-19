using MySeries.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MySeries.Controllers;

public abstract class MySeriesControllers : AbpControllerBase
{
    protected MySeriesControllers() 
    {
        LocalizationResource = typeof(MySeriesResource);
    }
}