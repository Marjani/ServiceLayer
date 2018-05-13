using FluentValidation;
using MyApp.Models.Admin.Users;

namespace MyApp.ServiceLayer.Users.Validators
{
    public class UserDeleteValidator : AbstractValidator<UserDeleteViewModel>
    {
        public UserDeleteValidator()
        {
            
        }
    }
}
