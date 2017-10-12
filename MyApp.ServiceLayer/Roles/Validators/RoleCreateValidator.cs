using FluentValidation;
using MyApp.Models.Admin.Roles;

namespace MyApp.ServiceLayer.Roles.Validators
{
    public class RoleCreateValidator : AbstractValidator<RoleCreateViewModel>
    {
        public RoleCreateValidator()
        {
            RuleFor(a => a.Name).NotNull();
        }
    }
}
