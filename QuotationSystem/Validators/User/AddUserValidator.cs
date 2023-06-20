using QuotationSystem.Data.Models;
using FluentValidation;
using QuotationSystem.Models.Account;
using Zero.Security;
using QuotationSystem.Data.Sessions;
using QuotationSystem.Data.Repositories;
using QuotationSystem.Models.User;

namespace QuotationSystem.Validators.User
{
    public class AddUserValidator : AbstractValidator<UserModalViewModel> //not using UserViewModel because of the nested object,
                                                             //maybe the validator can't validate something like UserViewModel.User.UserId (look at the UserViewModel class)
                                                             //previously using RuleFor(u => u.User.UserId) "u" is UserViewModel, The AddUserModal validation's HTML markup
                                                             //replace the whole page
                                                             //so in this case I validate the MUser model instead.
                                                             //TODO: make AddUserModal it's own model
    {
        private IUserRepository userRepository;
        private MUser user;
        public AddUserValidator(IUserRepository userRepository)
        {
            this.userRepository = userRepository;

            RuleFor(u => u.UserId)
                .Cascade(CascadeMode.Stop)
                 .NotEmpty()
                 .WithMessage("Please enter User ID")
                 .Length(1,50)
                 .WithMessage("Username too long")
                 .Must((model, userId) => !IsExistUser(userId))
                     .WithMessage("User ID already exist");
            RuleFor(u => u.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Username can't be empty");
            RuleFor(u => u.Department)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please select Department");
        }
        private bool IsExistUser(string userId)
        {
            user = userRepository.GetUserByUserId(userId);
            bool result = user is not null;
            return result;
        }
    }
}
