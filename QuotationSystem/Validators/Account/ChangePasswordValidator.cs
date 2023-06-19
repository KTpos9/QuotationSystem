using QuotationSystem.Data.Models;
using FluentValidation;
using QuotationSystem.Models.Account;
using Zero.Security;
using QuotationSystem.Data.Sessions;
using QuotationSystem.Data.Repositories;

namespace QuotationSystem.Validators.Account
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordViewModel>
    {
        private IUserRepository userRepository;
        private ISessionContext sessionContext;
        private MUser user;

        public ChangePasswordValidator(IUserRepository userRepository, ISessionContext sessionContext)
        {
            this.userRepository = userRepository;
            this.sessionContext = sessionContext;

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please enter password")
                .Must((model, password) => IsValidPassword(password))
                    .WithMessage("Password is incorrect.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Please enter password");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Please enter ConfirmPassword");

            When(x => string.IsNullOrEmpty(x.NewPassword) == false && string.IsNullOrEmpty(x.ConfirmPassword) == false, () =>
            {
                RuleFor(x => x.NewPassword)
                    .Equal(x => x.ConfirmPassword)
                    .WithMessage("Password and ConfirmPassword do not match.");
            });
        }
        private bool IsValidPassword(string password)
        {
            user = userRepository.GetUserByUserId(sessionContext.CurrentUser.Id);
            if (user == null)
            {
                return false;
            }
            var passwordHash = PasswordEncryption.Hash(password, "");
            return passwordHash.Equals(user.Password);
        }
    }
}
