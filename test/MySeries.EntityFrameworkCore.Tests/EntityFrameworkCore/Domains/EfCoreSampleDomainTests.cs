using MySeries.Samples;
using Xunit;

namespace MySeries.EntityFrameworkCore.Domains;

[Collection(MySeriesTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<MySeriesEntityFrameworkCoreTestModule>
{

}
