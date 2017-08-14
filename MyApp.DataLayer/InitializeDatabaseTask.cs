using System.Data.Entity;
using MyApp.DataLayer.Context;
using MyApp.Framework.Data;
using MyApp.Framework.Infrastructure.Tasks;

namespace MyApp.DataLayer
{
    public class InitializeDatabaseTask : IRunOnStartTask
    {
        private readonly IUnitOfWork _unitOfWork;

        public InitializeDatabaseTask(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Order => -1;

        public void Execute()
        {
            Database.SetInitializer<ApplicationDbContext>(null);
            _unitOfWork.ForceDatabaseInitialize();
        }
    }
}