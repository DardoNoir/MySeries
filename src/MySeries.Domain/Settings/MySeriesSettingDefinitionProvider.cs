using Volo.Abp.Settings;

namespace MySeries.Settings;

public class MySeriesSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(MySeriesSettings.MySetting1));
    }
}
