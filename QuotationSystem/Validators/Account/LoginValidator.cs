using FluentValidation;
using QuotationSystem.Data.Models;
using QuotationSystem.Data.Repositories;
using QuotationSystem.Models.Account;
using Zero.Security;

namespace WebApp.Validators.Account
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        private IUserRepository userRepository;
        private MUser user;

        public LoginValidator(IUserRepository userRepository)
        {
            this.userRepository = userRepository;

            RuleFor(x => x.UserId)
                 .Cascade(CascadeMode.Stop)
                 .NotEmpty()
                 .Must((model, userId) => IsExistUser(model.UserId))
                     .WithMessage("User not found.");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must((model, password) => IsValidPassword(password))
                    .WithMessage("Username or Password is incorrect.");
        }

        private bool IsExistUser(string userId)
        {
            user = userRepository.GetUserByUserId(userId);
            return user != null;
        }

        private bool IsValidPassword(string password)
        {
            if (user == null)
            {
                return false;
            }
            var passwordHash = PasswordEncryption.Hash(password, "");
            return passwordHash.Equals(user.Password);
        }
    }
}