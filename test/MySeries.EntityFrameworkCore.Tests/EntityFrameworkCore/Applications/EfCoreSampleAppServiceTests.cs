using MySeries.Samples;
using Xunit;

namespace MySeries.EntityFrameworkCore.Applications;

[Collection(MySeriesTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<MySeriesEntityFrameworkCoreTestModule>
{

}
