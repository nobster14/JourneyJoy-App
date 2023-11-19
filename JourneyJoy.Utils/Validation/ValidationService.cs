using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Utils.Validation
{
    public class ValidationService : IValidationService
    {
        #region Fields

        private PasswordValidator passwordValidator = new PasswordValidator();
        private EmailValidator emailValidator = new EmailValidator();

        #endregion

        #region Properties

        public PasswordValidator PasswordValidator => passwordValidator;

        public EmailValidator EmailValidator => emailValidator;

        #endregion
    }
}
