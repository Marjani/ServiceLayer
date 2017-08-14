using System;
using MyApp.Framework.Data;
using StructureMap.DynamicInterception;

namespace MyApp.Framework.Aspects.Transaction
{
    public class TransactionInterceptor : ISyncInterceptionBehavior
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionInterceptor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IMethodInvocationResult Intercept(ISyncMethodInvocation methodInvocation)
        {
            if (IsTransactionEnabled(methodInvocation)) return methodInvocation.InvokeNext();

            var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var result = methodInvocation.InvokeNext();

                transaction.Commit();

                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        private static bool IsTransactionEnabled(IMethodInvocation methodInvocation)
        {
            return methodInvocation.MethodInfo.IsDefined(typeof(TransactionalAttribute), true);
        }
    }
}
