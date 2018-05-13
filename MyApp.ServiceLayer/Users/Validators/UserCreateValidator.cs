using FluentValidation;
using MyApp.Models.Admin.Users;

namespace MyApp.ServiceLayer.Users.Validators
{
    public class UserCreateValidator : AbstractValidator<UserCreateViewModel>
    {
        public UserCreateValidator()
        {
            RuleFor(a => a.UserName).NotNull();
            RuleFor(a => a.Email).EmailAddress().NotNull();
            RuleFor(a => a.PasswordHash).NotNull();
            RuleFor(a => a.SecurityStamp).NotNull();
        }
    }
}
