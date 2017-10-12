using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace MyApp.IntegrationTests.Attributes
{
    public class HttpContextAttribute:Attribute,ITestAction
    {
        private FakeHttpContext.FakeHttpContext _httpContext;

        public void BeforeTest(ITest test)
        {
            _httpContext = new FakeHttpContext.FakeHttpContext();

        }

        public void AfterTest(ITest test)
        {
            _httpContext?.Dispose();
            _httpContext = null;
        }

        public ActionTargets Targets => ActionTargets.Test;
    }
}
