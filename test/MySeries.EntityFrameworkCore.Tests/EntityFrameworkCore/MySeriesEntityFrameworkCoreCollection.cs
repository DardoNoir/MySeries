using Xunit;

namespace MySeries.EntityFrameworkCore;

[CollectionDefinition(MySeriesTestConsts.CollectionDefinitionName)]
public class MySeriesEntityFrameworkCoreCollection : ICollectionFixture<MySeriesEntityFrameworkCoreFixture>
{

}
