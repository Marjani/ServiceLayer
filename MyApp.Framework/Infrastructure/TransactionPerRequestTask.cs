using System.Data;
using System.Web;
using MyApp.Framework.Data;
using MyApp.Framework.Infrastructure.Tasks;

namespace MyApp.Framework.Infrastructure
{
    public class TransactionPerRequestTask : IRunOnBeginRequestTask, IRunOnErrorTask, IRunOnEndRequestTask
    {
        #region Constructor

        public TransactionPerRequestTask(IUnitOfWork unitOfWork, HttpContextBase httpContext)
        {
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }

        #endregion

        #region Constants

        private const string Error = "TRANSACTION_PER_REQUEST_ERROR_KEY";
        private const string Transaction = "TRANSACTION_PER_REQUEST_TRANSACTION_KEY";

        #endregion

        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpContextBase _httpContext;

        #endregion

        #region Methods

        int IRunOnBeginRequestTask.Order => int.MaxValue;

        int IRunOnErrorTask.Order => int.MaxValue;

        int IRunOnEndRequestTask.Order => int.MaxValue;

        void IRunOnBeginRequestTask.Execute()
        {
            _httpContext.Items[Transaction] =
                _unitOfWork.BeginTransaction(IsolationLevel.Snapshot);
        }

        void IRunOnErrorTask.Execute()
        {
            _httpContext.Items[Error] = true;
        }

        void IRunOnEndRequestTask.Execute()
        {
            var transaction = (ITransaction) _httpContext.Items[Transaction];
            if (_httpContext.Items[Error] != null)
                transaction.Rollback();
            else
                transaction.Commit();
        }

        #endregion
    }
}