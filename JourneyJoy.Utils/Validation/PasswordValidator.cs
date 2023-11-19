using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JourneyJoy.Utils.Extensions;

namespace JourneyJoy.Utils.Validation
{
    public class PasswordValidator : IValidator<string>
    {
        public bool Validate(string input, out string error)
        {
            var stringBuilder = new StringBuilder();
            bool ret = true;

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception("Password should not be empty");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,25}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(input))
            {
                stringBuilder.AppendLine("Password should contain At least one lower case letter");
                ret = false;
            }
            if (!hasUpperChar.IsMatch(input))
            {
                stringBuilder.AppendLine("Password should contain At least one upper case letter");
                ret = false;
            }
            if (!hasMiniMaxChars.IsMatch(input))
            {
                stringBuilder.AppendLine("Password should not be less than 8 or greater than 25 characters");
                ret = false;
            }
            if (!hasNumber.IsMatch(input))
            {
                stringBuilder.AppendLine("Password should contain At least one numeric value");
                ret = false;
            }

            if (!hasSymbols.IsMatch(input))
            {
                stringBuilder.AppendLine("Password should contain At least one special case characters");
                ret = false;
            }

            stringBuilder.TrimEnd();
            error = stringBuilder.ToString();

            return ret;
        }
    }
}
