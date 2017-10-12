using MyApp.IntegrationTests.Attributes;
using NUnit.Framework;

namespace MyApp.IntegrationTests
{
    [TestFixture]
    [AutoRollback]
    [HttpContext]
    public class ServiceTestsBase
    {
        
    }
}
