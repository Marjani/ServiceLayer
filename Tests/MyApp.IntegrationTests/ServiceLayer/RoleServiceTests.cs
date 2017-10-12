using System.Data.Entity.Infrastructure;
using System.Linq;
using FluentValidation;
using MyApp.DomainClasses.Roles;
using MyApp.Framework.Data;
using MyApp.Framework.Infrastructure;
using MyApp.IntegrationTests.Attributes;
using MyApp.IntegrationTests.InitialData;
using MyApp.Models.Admin.Roles;
using MyApp.ServiceLayer.Roles;
using NUnit.Framework;
using Shouldly;

namespace MyApp.IntegrationTests.ServiceLayer
{
    [TestFixture]
    [AutoRollback]
    [HttpContext]
    public class RoleServiceTests
    {
        private IRoleApplicationService _roleService;

        [SetUp]
        public void Init()
        {
        }

        [TearDown]
        public void Clean()
        {
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            _roleService = IoC.Resolve<IRoleApplicationService>();

            using (var uow = IoC.Resolve<IUnitOfWork>())
            {
                RoleInitialDataBuilder.Build(uow);
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }

        [Test]
        [TestCase("Role1")]
        public void Should_Create_New_Role(string role)
        {
            var viewModel = new RoleCreateViewModel
            {
                Name = role
            };

            _roleService.Create(viewModel);

            using (var context = IoC.Resolve<IUnitOfWork>())
            {
                var user = context.Set<Role>().FirstOrDefault(a => a.Name == role);
                user.ShouldNotBeNull();
            }
        }

        [Test]
        [TestCase("Admin")]
        public void Should_Not_Create_New_Role_With_Admin_Name(string role)
        {
            var viewModel = new RoleCreateViewModel
            {
                Name = role.ToUpperInvariant()
            };

            Assert.Throws<DbUpdateException>(() => _roleService.Create(viewModel));
        }

        [Test]
        public void Should_Delete_Role()
        {
            var viewModel = new RoleDeleteViewModel { };
        }

        [Test]
        public void Should_AdminRole_Exists()
        {
            using (var context = IoC.Resolve<IUnitOfWork>())
            {
                var user = context.Set<Role>().FirstOrDefault(a => a.Name == "Admin");
                user.ShouldNotBeNull();
            }
        }

        [Test]
        public void Should_Not_Create_New_Role_Without_Name()
        {
            using (new FakeHttpContext.FakeHttpContext())
            {
                Assert.Throws<ValidationException>(() => _roleService.Create(new RoleCreateViewModel { Name = null }));
            }
        }
    }
}

