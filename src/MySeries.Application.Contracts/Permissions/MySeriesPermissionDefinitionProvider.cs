using MySeries.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace MySeries.Permissions;

public class MySeriesPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(MySeriesPermissions.GroupName);

        var booksPermission = myGroup.AddPermission(MySeriesPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(MySeriesPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(MySeriesPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(MySeriesPermissions.Books.Delete, L("Permission:Books.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(MySeriesPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MySeriesResource>(name);
    }
}
