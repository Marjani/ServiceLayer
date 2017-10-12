using System;
using System.Transactions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace MyApp.IntegrationTests.Attributes
{
    public class AutoRollbackAttribute : Attribute, ITestAction
    {
        private TransactionScope _scope;

        public void BeforeTest(ITest test)
        {
            _scope = new TransactionScope(TransactionScopeOption.RequiresNew,new TransactionOptions {IsolationLevel = IsolationLevel.Snapshot});
        }

        public void AfterTest(ITest test)
        {
            _scope?.Dispose();
            _scope = null;
        }

        public ActionTargets Targets => ActionTargets.Test;
    }
}
