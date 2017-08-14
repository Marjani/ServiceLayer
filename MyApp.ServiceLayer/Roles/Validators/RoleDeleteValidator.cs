using FluentValidation;
using MyApp.Models.Admin.Roles;

namespace MyApp.ServiceLayer.Roles.Validators
{
    public class RoleDeleteValidator : AbstractValidator<RoleDeleteViewModel>
    {
        public RoleDeleteValidator()
        {
            
        }
    }
}
