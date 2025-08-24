using MySeries.Books;
using Xunit;

namespace MySeries.EntityFrameworkCore.Applications.Books;

[Collection(MySeriesTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : BookAppService_Tests<MySeriesEntityFrameworkCoreTestModule>
{

}