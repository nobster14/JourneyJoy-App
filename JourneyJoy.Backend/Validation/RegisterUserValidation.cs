using JourneyJoy.Repository;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using JourneyJoy.Model.Requests;
using Microsoft.IdentityModel.Tokens;
using JourneyJoy.Contracts;
using JourneyJoy.Utils.Validation;

namespace JourneyJoy.Backend.Validation
{
    public class RegisterUserValidation : IActionFilter
    {
        #region Private fields

        private IValidationService validationService;
        private IRepositoryWrapper repositoryWrapper;

        #endregion
        public RegisterUserValidation(IValidationService validationService, IRepositoryWrapper repositoryWrapper)
        {
            this.validationService = validationService;
            this.repositoryWrapper = repositoryWrapper;
        }
        #region Constructors

        #endregion

        #region Interface methods
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var param = context.ActionArguments.SingleOrDefault(p => p.Value is RegisterUserRequest);
            if (param.Value == null)
            {
                context.Result = new BadRequestObjectResult("Request is null");
                return;
            }

            StringBuilder stringBuilder = new StringBuilder();
            var req = (RegisterUserRequest)param.Value;

            if (!req.Email.IsNullOrEmpty())
                if (!validationService.EmailValidator.Validate(req.Email, out var error1))
                    stringBuilder.AppendLine(error1);
            if (!req.Password.IsNullOrEmpty())
                if (!validationService.PasswordValidator.Validate(req.Password, out var error2))
                    stringBuilder.AppendLine(error2);

                if (repositoryWrapper.UserRepository.FindUserByEmail(req.Email) != null)
                    stringBuilder.AppendLine("User with this email exists");

            if (stringBuilder.Length > 0)
                context.Result = new BadRequestObjectResult($"{stringBuilder}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        #endregion
    }
}
