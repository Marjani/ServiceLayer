using System.Data.Entity.Migrations;
using MyApp.DomainClasses.Roles;
using MyApp.Framework.Data;

namespace MyApp.IntegrationTests.InitialData
{
    public static class RoleInitialDataBuilder
    {
        public static void Build(IUnitOfWork unitOfWork)
        {
            using (new FakeHttpContext.FakeHttpContext())
            {
                unitOfWork.Set<Role>().AddOrUpdate(a => a.Name, new Role { Name = "Admin", IsSystemEntry = true });
                unitOfWork.SaveChanges();
            }
        }
    }
}
